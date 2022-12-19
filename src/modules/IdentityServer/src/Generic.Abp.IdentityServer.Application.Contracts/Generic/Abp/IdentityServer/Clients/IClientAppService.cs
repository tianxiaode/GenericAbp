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
    Task<ListResultDto<ClientPropertyDto>> GetPropertiesAsync(Guid id);
    Task AddPropertyAsync(Guid id, ClientPropertyCreateInput input);
    Task RemovePropertyAsync(Guid id, ClientPropertyDeleteInput input);
    Task<ListResultDto<ClientIdentityProviderRestrictionDto>> GetIdentityProviderRestrictionsAsync(Guid id);
    Task AddIdentityProviderRestrictionAsync(Guid id, ClientIdentityProviderRestrictionCreateInput input);
    Task RemoveIdentityProviderRestrictionAsync(Guid id, ClientIdentityProviderRestrictionDeleteInput input);
    Task UpdateEnabledAsync(Guid id, bool value);
    Task UpdateRequireClientSecretAsync(Guid id, bool value);
    Task UpdateRequireRequestObjectAsync(Guid id, bool value);
    Task UpdateRequireConsentAsync(Guid id, bool value);
    Task UpdateAllowRememberConsentAsync(Guid id, bool value);
    Task UpdateAlwaysIncludeUserClaimsInIdTokenAsync(Guid id, bool value);
    Task UpdateAlwaysRequirePkceAsync(Guid id, bool value);
    Task UpdateAllowPlainTextPkceAsync(Guid id, bool value);
    Task UpdateAllowAccessTokensViaBrowserAsync(Guid id, bool value);
    Task UpdateFrontChannelLogoutSessionRequiredAsync(Guid id, bool value);
    Task UpdateBackChannelLogoutSessionRequiredAsync(Guid id, bool value);
    Task UpdateAllowOfflineAccessAsync(Guid id, bool value);
    Task UpdateUpdateAccessTokenClaimsOnRefreshAsync(Guid id, bool value);
    Task UpdateEnableLocalLoginAsync(Guid id, bool value);
    Task UpdateIncludeJwtIdAsync(Guid id, bool value);
    Task UpdateAlwaysSendClientClaimsAsync(Guid id, bool value);


}