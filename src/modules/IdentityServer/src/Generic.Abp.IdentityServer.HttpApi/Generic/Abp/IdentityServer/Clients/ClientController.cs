using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.IdentityServer.Clients;

[Area("IdentityServer")]
[ControllerName("IdentityServer")]
[Route("api/clients")]

public class ClientController: IdentityServerController,IClientAppService
{
    public ClientController(IClientAppService clientAppService)
    {
        ClientAppService = clientAppService;
    }

    protected IClientAppService ClientAppService { get; }
    
    [HttpGet]
    [Route("{id:guid}")]
    public Task<ClientDto> GetAsync(Guid id)
    {
        return ClientAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<ClientDto>> GetListAsync(ClientGetListDto input)
    {
        return ClientAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<ClientDto> CreateAsync(ClientCreateInput input)
    {
        return ClientAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<ClientDto> UpdateAsync(Guid id, [FromBody] ClientUpdateInput input)
    {
        return ClientAppService.GetAsync(id);
    }

    [HttpDelete]
    public Task<ListResultDto<ClientDto>> DeleteAsync([FromBody]List<Guid> ids)
    {
        return ClientAppService.DeleteAsync(ids);
    }

    [HttpGet]
    [Route("{id:guid}/claims")]
    public Task<ListResultDto<ClientClaimDto>> GetClaimsAsync(Guid id)
    {
        return ClientAppService.GetClaimsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/claims")]
    public Task AddClaimAsync(Guid id, [FromBody] ClientClaimCreateInput input)
    {
        return ClientAppService.AddClaimAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/claims")]
    public Task RemoveClaimAsync(Guid id,[FromBody] ClientClaimDeleteInput input)
    {
        return ClientAppService.RemoveClaimAsync(id , input);
    }

    [HttpGet]
    [Route("{id:guid}/cors-origins")]
    public Task<ListResultDto<ClientCorsOriginDto>> GetCorsOriginsAsync(Guid id)
    {
        return ClientAppService.GetCorsOriginsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/cors-origins")]
    public Task AddCorsOriginAsync(Guid id, [FromBody] ClientCorsOriginCreateInput input)
    {
        return ClientAppService.AddCorsOriginAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/cors-origins")]
    public Task RemoveCorsOriginAsync(Guid id,[FromBody] ClientCorsOriginDeleteInput input)
    {
                return ClientAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("{id:guid}/grant-types")]
    public Task<ListResultDto<ClientGrantTypeDto>> GetGrantTypesAsync(Guid id)
    {
        return ClientAppService.GetGrantTypesAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/grant-types")]
    public Task AddGrantTypeAsync(Guid id,[FromBody] ClientGrantTypeCreateInput input)
    {
        return ClientAppService.AddGrantTypeAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/grant-types")]
    public Task RemoveGrantTypeAsync(Guid id, ClientGrantTypeDeleteInput input)
    {
        return ClientAppService.RemoveGrantTypeAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/scopes")]
    public Task<ListResultDto<ClientScopeDto>> GetScopesAsync(Guid id)
    {
        return ClientAppService.GetScopesAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/scopes")]
    public Task AddScopeAsync(Guid id, [FromBody] ClientScopeCreateInput input)
    {
        return ClientAppService.AddScopeAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/scopes")]
    public Task RemoveScopeAsync(Guid id, [FromBody] ClientScopeDeleteInput input)
    {
        return ClientAppService.RemoveScopeAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/secrets")]
    public Task<ListResultDto<ClientSecretDto>> GetClientSecretsAsync(Guid id)
    {
        return ClientAppService.GetClientSecretsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/secrets")]
    public Task AddSecretAsync(Guid id, [FromBody] ClientSecretCreateInput input)
    {
        return ClientAppService.AddSecretAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/secrets")]
    public Task RemoveSecretAsync(Guid id, [FromBody] ClientSecretDeleteInput input)
    {
        return ClientAppService.RemoveSecretAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/logout-redirect-uris")]
    public Task<ListResultDto<ClientPostLogoutRedirectUriDto>> GetPostLogoutRedirectUrisAsync(Guid id)
    {
        return ClientAppService.GetPostLogoutRedirectUrisAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/logout-redirect-uris")]
    public Task AddPostLogoutRedirectUriAsync(Guid id, [FromBody] ClientPostLogoutRedirectUriCreateInput input)
    {
        return ClientAppService.AddPostLogoutRedirectUriAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/logout-redirect-uris")]
    public Task RemovePostLogoutRedirectUriAsync(Guid id,[FromBody] ClientPostLogoutRedirectUriDeleteInput input)
    {
        return ClientAppService.RemovePostLogoutRedirectUriAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/redirect-uris")]
    public Task<ListResultDto<ClientRedirectUriDto>> GetRedirectUrisAsync(Guid id)
    {
        return ClientAppService.GetRedirectUrisAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/redirect-uris")]
    public Task AddRedirectUriAsync(Guid id,[FromBody] ClientRedirectUriCreateInput input)
    {
        return ClientAppService.AddRedirectUriAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/redirect-uris")]
    public Task RemoveRedirectUriAsync(Guid id,[FromBody] ClientRedirectUriDeleteInput input)
    {
        return ClientAppService.RemoveRedirectUriAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/properties")]
    public Task<ListResultDto<ClientPropertyDto>> GetPropertiesAsync(Guid id)
    {
        return ClientAppService.GetPropertiesAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/properties")]
    public Task AddPropertyAsync(Guid id, ClientPropertyCreateInput input)
    {
        return ClientAppService.AddPropertyAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/properties")]
    public Task RemovePropertyAsync(Guid id, ClientPropertyDeleteInput input)
    {
        return ClientAppService.RemovePropertyAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/identity-provider-restrictions")]
    public Task<ListResultDto<ClientIdentityProviderRestrictionDto>> GetIdentityProviderRestrictionsAsync(Guid id)
    {
        return ClientAppService.GetIdentityProviderRestrictionsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/identity-provider-restrictions")]
    public Task AddIdentityProviderRestrictionAsync(Guid id, ClientIdentityProviderRestrictionCreateInput input)
    {
        return ClientAppService.AddIdentityProviderRestrictionAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/identity-provider-restrictions")]
    public Task RemoveIdentityProviderRestrictionAsync(Guid id, ClientIdentityProviderRestrictionDeleteInput input)
    {
        return ClientAppService.RemoveIdentityProviderRestrictionAsync(id, input);
    }

    [RemoteService(false)]
    public Task UpdateEnabledAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/enable")]
    public Task Enable(Guid id)
    {
        return ClientAppService.UpdateEnabledAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public Task Disable(Guid id)
    {
        return ClientAppService.UpdateEnabledAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateRequireClientSecretAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/require-client-secret")]
    public Task RequireClientSecretAsync(Guid id)
    {
        return ClientAppService.UpdateRequireClientSecretAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/optional-client-secret")]
    public Task OptionalClientSecretAsync(Guid id)
    {
        return ClientAppService.UpdateRequireClientSecretAsync(id, false);
    }

    [RemoteService(false)]
    public Task UpdateRequireRequestObjectAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/require-request-object")]
    public Task RequireRequestObjectAsync(Guid id)
    {
        return ClientAppService.UpdateRequireRequestObjectAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/optional-request-object")]
    public Task OptionalRequestObjectAsync(Guid id)
    {
        return ClientAppService.UpdateRequireRequestObjectAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateRequireConsentAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/require-consent")]
    public Task RequireConsentAsync(Guid id)
    {
        return ClientAppService.UpdateRequireConsentAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/optional-consent")]
    public Task OptionalConsentAsync(Guid id)
    {
        return ClientAppService.UpdateRequireConsentAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateAllowRememberConsentAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/allow-remember-consent")]
    public Task AllowRememberConsentAsync(Guid id)
    {
        return ClientAppService.UpdateAllowRememberConsentAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/forbid-remember-consent")]
    public Task ForbidRememberConsentAsync(Guid id)
    {
        return ClientAppService.UpdateAllowRememberConsentAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateAlwaysIncludeUserClaimsInIdTokenAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/always-include-user-claims-in-id-token")]
    public Task AlwaysIncludeUserClaimsInIdTokenAsync(Guid id)
    {
        return ClientAppService.UpdateAlwaysIncludeUserClaimsInIdTokenAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/never-include-user-claims-in-id-token")]
    public Task NeverIncludeUserClaimsInIdTokenAsync(Guid id)
    {
        return ClientAppService.UpdateAlwaysIncludeUserClaimsInIdTokenAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateAlwaysRequirePkceAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/require-pkce")]
    public Task RequirePkceAsync(Guid id)
    {
        return ClientAppService.UpdateAlwaysRequirePkceAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/optional-pkce")]
    public Task OptionalPkceAsync(Guid id)
    {
        return ClientAppService.UpdateAlwaysRequirePkceAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateAllowPlainTextPkceAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/allow-plain-text-pkce")]
    public Task AllowPlainTextPkceAsync(Guid id)
    {
        return ClientAppService.UpdateAllowPlainTextPkceAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/forbid-plain-text-pkce")]
    public Task ForbidPlainTextPkceAsync(Guid id)
    {
        return ClientAppService.UpdateAllowPlainTextPkceAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateAllowAccessTokensViaBrowserAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/allow-access-tokens-via-browser")]
    public Task AllowAccessTokensViaBrowserAsync(Guid id)
    {
        return ClientAppService.UpdateAllowAccessTokensViaBrowserAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/forbid-access-tokens-via-browser")]
    public Task ForbidAccessTokensViaBrowserAsync(Guid id)
    {
        return ClientAppService.UpdateAllowAccessTokensViaBrowserAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateFrontChannelLogoutSessionRequiredAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/front-channel-logout-session-required")]
    public Task FrontChannelLogoutSessionRequiredAsync(Guid id)
    {
        return ClientAppService.UpdateFrontChannelLogoutSessionRequiredAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/front-channel-logout-session-optional")]
    public Task FrontChannelLogoutSessionOptionalAsync(Guid id)
    {
        return ClientAppService.UpdateFrontChannelLogoutSessionRequiredAsync(id, false);
    }

    [RemoteService(false)]
    public Task UpdateBackChannelLogoutSessionRequiredAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/back-channel-logout-session-required")]
    public Task BackChannelLogoutSessionRequiredAsync(Guid id)
    {
        return ClientAppService.UpdateBackChannelLogoutSessionRequiredAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/back-channel-logout-session-optional")]
    public Task BackChannelLogoutSessionOptionalAsync(Guid id)
    {
        return ClientAppService.UpdateBackChannelLogoutSessionRequiredAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateAllowOfflineAccessAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/allow-offline-access")]
    public Task AllowOfflineAccessAsync(Guid id)
    {
        return ClientAppService.UpdateAllowOfflineAccessAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/forbid-offline-access")]
    public Task ForbidOfflineAccessAsync(Guid id)
    {
        return ClientAppService.UpdateAllowOfflineAccessAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateUpdateAccessTokenClaimsOnRefreshAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/update-access-token-claims-on-refresh")]
    public Task UpdateAccessTokenClaimsOnRefreshAsync(Guid id)
    {
        return ClientAppService.UpdateUpdateAccessTokenClaimsOnRefreshAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/keep-access-token-claims-on-refresh")]
    public Task KeepAccessTokenClaimsOnRefreshAsync(Guid id)
    {
        return ClientAppService.UpdateUpdateAccessTokenClaimsOnRefreshAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateEnableLocalLoginAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/enable-local-login")]
    public Task EnableLocalLoginAsync(Guid id)
    {
        return ClientAppService.UpdateEnableLocalLoginAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/disable-local-login")]
    public Task DisableLocalLoginAsync(Guid id)
    {
        return ClientAppService.UpdateEnableLocalLoginAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateIncludeJwtIdAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/include-jwt-id")]
    public Task IncludeJwtIdAsync(Guid id)
    {
        return ClientAppService.UpdateIncludeJwtIdAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/exclude-jwt-id")]
    public Task ExcludeJwtIdAsync(Guid id)
    {
        return ClientAppService.UpdateIncludeJwtIdAsync(id, false);
    }

    [RemoteService(false)]
    public Task UpdateAlwaysSendClientClaimsAsync(Guid id, bool value)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/always-send-client-claims")]
    public Task AlwaysSendClientClaimsAsync(Guid id)
    {
        return ClientAppService.UpdateAlwaysSendClientClaimsAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/never-send-client-claims")]
    public Task NeverSendClientClaimsAsync(Guid id)
    {
        return ClientAppService.UpdateAlwaysSendClientClaimsAsync(id, false);
    }

}