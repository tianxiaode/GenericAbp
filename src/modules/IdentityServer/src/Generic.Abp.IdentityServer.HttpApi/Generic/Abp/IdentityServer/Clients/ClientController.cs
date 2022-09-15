using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
}