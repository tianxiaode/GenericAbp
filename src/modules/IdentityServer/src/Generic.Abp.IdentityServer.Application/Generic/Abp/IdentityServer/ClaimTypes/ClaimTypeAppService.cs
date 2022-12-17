using Generic.Abp.IdentityServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Uow;

namespace Generic.Abp.IdentityServer.ClaimTypes
{
    [RemoteService(false)]
    public class ClaimTypeAppService : IdentityServerAppService, IClaimTypeAppService
    {
        protected IIdentityClaimTypeRepository Repository { get; }

        public ClaimTypeAppService(IIdentityClaimTypeRepository repository)
        {
            Repository = repository;
        }

        [UnitOfWork]
        [Authorize(IdentityServerPermissions.ClaimTypes.Default)]
        public virtual async Task<ListResultDto<ClaimTypeDto>> GetListAsync()
        {
            var sorting = $"{nameof(AbpClaimTypes.Name)}";
            var list = await Repository.GetPagedListAsync(0, 100, sorting);
            return new ListResultDto<ClaimTypeDto>(list.Select(m => new ClaimTypeDto(m.Name)).ToList());
        }


    }
}
