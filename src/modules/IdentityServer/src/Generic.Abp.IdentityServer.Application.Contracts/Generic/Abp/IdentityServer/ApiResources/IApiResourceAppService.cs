using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.IdentityServer.ApiResources;

public interface IApiResourceAppService: IApplicationService
{
    Task<ApiResourceDto> GetAsync(Guid id);
    Task<PagedResultDto<ApiResourceDto>> GetListAsync(ApiResourceGetListDto input);
    Task<ApiResourceDto> CreateAsync(ApiResourceCreateInput input);
    Task<ApiResourceDto> UpdateAsync(Guid id, ApiResourceUpdateInput input);
    Task UpdateEnableAsync(Guid id, bool enable);
    Task<ListResultDto<ApiResourceDto>> DeleteAsync(List<Guid> ids);
    Task<ListResultDto<ApiResourceClaimDto>> GetClaimsAsync(Guid id);
    Task AddClaimAsync(Guid id, ApiResourceClaimCreateInput input);
    Task RemoveClaimAsync(Guid id, ApiResourceClaimDeleteInput input);
    Task<ListResultDto<ApiResourceScopeDto>> GetScopesAsync(Guid id);
    Task AddScopeAsync(Guid id, ApiResourceScopeCreateInput input);
    Task RemoveScopeAsync(Guid id, ApiResourceScopeDeleteInput input);
    Task<ListResultDto<ApiResourceSecretDto>> GetClientSecretsAsync(Guid id);
    Task AddSecretAsync(Guid id, ApiResourceSecretCreateInput input);
    Task RemoveSecretAsync(Guid id, ApiResourceSecretDeleteInput input);

}