using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Enumeration
{
    public class EnumerationAppService : ApplicationService, IEnumerationAppService
    {

        protected EnumerationOptions Options { get; }
        public EnumerationAppService(
            IOptions<EnumerationOptions> enumOptions
            )
        {
            Options = enumOptions.Value;
        }

        public virtual Task<ListResultDto<EnumDto>> GetEnumsAsync()
        {
            var list = new List<EnumDto>();
            foreach (var type in Options.Resources)
            {
                var fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.Static).Where(m =>
                {
                    var fi = (Enumeration)m.GetValue(null);
                    if (fi == null) return false;
                    if (fi.Permission != null)
                    {
                        return fi.Permission.Any(policyName =>
                            AuthorizationService.IsGrantedAsync(policyName).GetAwaiter().GetResult());
                    }

                    return !fi.IsPrivate;

                });
                list.AddRange(
                    from info in fieldInfo
                    let e = ((Enumeration)info.GetValue(null))
                    select new EnumDto()
                    {
                        Id = $"{type.Name.ToCamelCase()}-{e.Id}",
                        Key = info.Name.ToCamelCase(),
                        Type = type.Name.ToCamelCase(),
                        Text = $"{type.Name}.{info.Name}",
                        Value = e.Id,
                        IsDefault = e.IsDefault,
                        ResourceName = e.ResourceName
                    }

                );
            }

            return Task.FromResult(new ListResultDto<EnumDto>(list));
        }
    }
}
