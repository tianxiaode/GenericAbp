using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.IdentityServer.Clients;

public interface IClientAppService: IApplicationService
{
    Task<ClientDto> GetAsync(Guid id);
    Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetListDto input);
    Task<ClientDto> CreateAsync(ClientCreateInput input);
    Task<ClientDto> UpdateAsync(Guid id, ClientUpdateInput input);
    Task<ListResultDto<ClientDto>> DeleteAsync(List<Guid> ids);
    Task<ListResultDto<ClientClaimDto>> GetClaimsAsync(Guid id);
    Task AddClaimAsync(Guid id, ClientClaimCreateInput input);
    Task RemoveClaimAsync(Guid id, ClientClaimDeleteInput input);
    Task<ListResultDto<ClientCorsOriginDto>> GetCorsOriginsAsync(Guid id);
    Task AddCorsOriginAsync(Guid id, ClientCorsOriginCreateInput input);
    Task RemoveCorsOriginAsync(Guid id, ClientCorsOriginDeleteInput input);
    Task<ListResultDto<ClientGrantTypeDto>> GetGrantTypesAsync(Guid id);
    Task AddGrantTypeAsync(Guid id, ClientGrantTypeCreateInput input);
    Task RemoveGrantTypeAsync(Guid id, ClientGrantTypeDeleteInput input);
    Task<ListResultDto<ClientScopeDto>> GetScopesAsync(Guid id);
    Task AddScopeAsync(Guid id, ClientScopeCreateInput input);
    Task RemoveScopeAsync(Guid id, ClientScopeDeleteInput input);
    Task<ListResultDto<ClientSecretDto>> GetClientSecretsAsync(Guid id);
    Task AddSecretAsync(Guid id, ClientSecretCreateInput input);
    Task RemoveSecretAsync(Guid id, ClientSecretDeleteInput input);
    Task<ListResultDto<ClientPostLogoutRedirectUriDto>> GetPostLogoutRedirectUrisAsync(Guid id);
    Task AddPostLogoutRedirectUriAsync(Guid id, ClientPostLogoutRedirectUriCreateInput input);
    Task RemovePostLogoutRedirectUriAsync(Guid id, ClientPostLogoutRedirectUriDeleteInput input);
    Task<ListResultDto<ClientRedirectUriDto>> GetRedirectUrisAsync(Guid id);
    Task AddRedirectUriAsync(Guid id, ClientRedirectUriCreateInput input);
    Task RemoveRedirectUriAsync(Guid id, ClientRedirectUriDeleteInput input);
}