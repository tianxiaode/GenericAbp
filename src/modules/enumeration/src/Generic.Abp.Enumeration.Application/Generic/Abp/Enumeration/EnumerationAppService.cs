using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Uow;
using static System.Reflection.BindingFlags;

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

        public virtual async Task<ListResultDto<EnumDto>> GetEnumsAsync()
        {
            var list = new List<EnumDto>();
            foreach (var item in Options.Resources)
            {
                var itemName = item.Name.ToCamelCase();
                var fieldInfos = item.GetFields(Public | Static);
                foreach (var fieldInfo in fieldInfos)
                {
                    var obj = fieldInfo.GetValue(null);
                    switch (obj)
                    {
                        case IEnumeration<int> enumeration:
                        {
                            await MapToDtoAsync(itemName, enumeration, enumeration.Value, list);
                            break;
                        }
                        case IEnumeration<byte> enumeration:
                        {
                            await MapToDtoAsync(itemName, enumeration, enumeration.Value, list);
                            break;
                        }
                    }
                }

            }

            return new ListResultDto<EnumDto>(list);
        }

        [UnitOfWork]
        protected virtual async Task MapToDtoAsync<T>(string name,IEnumeration<T> enumeration, int value, List<EnumDto> list)
        {
            if (enumeration.IsPrivate) return;
            var permission = enumeration.Permission;
            var allow = false;
            if (permission != null)
            {
                foreach (var s in permission)
                {
                    if(string.IsNullOrEmpty(s)) continue;
                    if (await AuthorizationService.IsGrantedAnyAsync(s))
                    {
                        allow = true;
                    }
                    if(allow) break;
                }
            }
            else
            {
                allow = true;
            }
            if (!allow) return;

            var dto = new EnumDto($"{name}-{enumeration.Value}", name, enumeration.Name, value,
                enumeration.IsDefault, enumeration.ResourceName, enumeration.Order);
            list.Add(dto);
        }
    }
}
