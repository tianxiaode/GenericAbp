using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.IdentityServer.ApiScopes;

public interface IApiScopeAppService: IApplicationService
{
    Task<ApiScopeDto> GetAsync(Guid id);
    Task<PagedResultDto<ApiScopeDto>> GetListAsync(ApiScopeGetListDto input);
    Task<ApiScopeDto> CreateAsync(ApiScopeCreateInput input);
    Task<ApiScopeDto> UpdateAsync(Guid id, ApiScopeUpdateInput input);
    Task<ListResultDto<ApiScopeDto>> DeleteAsync(List<Guid> ids);
    Task<ListResultDto<ApiScopeClaimDto>> GetClaimsAsync(Guid id);
    Task AddClaimAsync(Guid id, ApiScopeClaimCrateInput input);
    Task RemoveClaimAsync(Guid id, ApiScopeClaimDeleteInput input);
}