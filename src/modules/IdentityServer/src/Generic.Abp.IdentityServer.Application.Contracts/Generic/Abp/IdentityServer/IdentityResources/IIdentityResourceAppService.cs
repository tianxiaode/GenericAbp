using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.IdentityServer.IdentityResources;

public interface IIdentityResourceAppService: IApplicationService
{
    Task<IdentityResourceDto> GetAsync(Guid id);
    Task<PagedResultDto<IdentityResourceDto>> GetListAsync(IdentityResourceGetListDto input);
    Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateInput input);
    Task<IdentityResourceDto> UpdateAsync(Guid id, IdentityResourceUpdateInput input);
    Task<ListResultDto<IdentityResourceDto>> DeleteAsync(List<Guid> ids);
    Task<ListResultDto<IdentityResourceClaimDto>> GetClaimsAsync(Guid id);
    Task AddClaimAsync(Guid id, IdentityResourceClaimCrateInput input);
    Task RemoveClaimAsync(Guid id, IdentityResourceClaimDeleteInput input);
}