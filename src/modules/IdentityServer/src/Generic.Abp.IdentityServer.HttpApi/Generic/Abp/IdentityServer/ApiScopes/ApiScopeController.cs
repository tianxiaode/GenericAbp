using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.IdentityServer.ApiScopes;

[Area("IdentityServer")]
[ControllerName("IdentityServer")]
[Route("api/api-scopes")]

public class ApiScopeController: IdentityServerController, IApiScopeAppService
{
    public ApiScopeController(IApiScopeAppService apiScopeAppService)
    {
        ApiScopeAppService = apiScopeAppService;
    }

    protected IApiScopeAppService ApiScopeAppService { get;  }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<ApiScopeDto> GetAsync(Guid id)
    {
        return ApiScopeAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<ListResultDto<ApiScopeDto>> GetListAsync()
    {
        return ApiScopeAppService.GetListAsync();
    }

    [HttpPost]
    public Task<ApiScopeDto> CreateAsync(ApiScopeCreateInput input)
    {
        return ApiScopeAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<ApiScopeDto> UpdateAsync(Guid id,[FromBody] ApiScopeUpdateInput input)
    {
        return ApiScopeAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task<ListResultDto<ApiScopeDto>> DeleteAsync([FromBody]List<Guid> ids)
    {
        return ApiScopeAppService.DeleteAsync(ids);
    }

    [HttpGet]
    [Route("{id:guid}/claims")]
    public Task<ListResultDto<ApiScopeClaimDto>> GetClaimsAsync(Guid id)
    {
        return ApiScopeAppService.GetClaimsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/claims")]
    public Task AddClaimAsync(Guid id, [FromBody] ApiScopeClaimCrateInput input)
    {
        return ApiScopeAppService.AddClaimAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/claims")]
    public Task RemoveClaimAsync(Guid id, [FromBody] ApiScopeClaimDeleteInput input)
    {
        return ApiScopeAppService.RemoveClaimAsync(id, input);
    }

    [RemoteService(false)]
    public Task UpdateEnableAsync(Guid id, bool enable)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/enable")]
    public Task Enable(Guid id)
    {
        return ApiScopeAppService.UpdateEnableAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public Task Disable(Guid id)
    {
        return ApiScopeAppService.UpdateEnableAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateShowInDiscoveryDocumentAsync(Guid id, bool isShow)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/show")]
    public Task Show(Guid id)
    {
        return ApiScopeAppService.UpdateShowInDiscoveryDocumentAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/hide")]
    public Task Hide(Guid id)
    {
        return ApiScopeAppService.UpdateShowInDiscoveryDocumentAsync(id, false);
    }


    [RemoteService(false)]
    public Task UpdateEmphasizeAsync(Guid id, bool isEmphasize)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{id:guid}/emphasize")]
    public Task Emphasize(Guid id)
    {
        return ApiScopeAppService.UpdateEmphasizeAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/understate")]
    public Task Understate(Guid id)
    {
        return ApiScopeAppService.UpdateEmphasizeAsync(id, false);
    }

    [RemoteService(false)]
    public Task UpdateRequiredAsync(Guid id, bool isEmphasize)
    {
        throw new NotImplementedException();
    }

        [HttpPut]
    [Route("{id:guid}/required")]
    public Task Required(Guid id)
    {
        return ApiScopeAppService.UpdateRequiredAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/optional")]
    public Task Optional(Guid id)
    {
        return ApiScopeAppService.UpdateRequiredAsync(id, false);
    }

     [HttpGet]
    [Route("{id:guid}/properties")]
   public Task<ListResultDto<ApiScopePropertyDto>> GetPropertiesAsync(Guid id)
    {
        return ApiScopeAppService.GetPropertiesAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/properties")]
    public Task AddPropertyAsync(Guid id, [FromBody] ApiScopePropertyCreateInput input)
    {
        return ApiScopeAppService.AddPropertyAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/properties")]
    public Task RemovePropertyAsync(Guid id,[FromBody] ApiScopePropertyDeleteInput input)
    {
        return ApiScopeAppService.RemovePropertyAsync(id, input);
    }
}