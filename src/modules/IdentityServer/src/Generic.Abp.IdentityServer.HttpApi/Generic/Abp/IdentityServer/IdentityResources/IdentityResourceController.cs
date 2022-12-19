using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.IdentityServer.IdentityResources;

[Area("IdentityServer")]
[ControllerName("IdentityServer")]
[Route("api/identity-resources")]

public class IdentityResourceController : IdentityServerController, IIdentityResourceAppService
{
    public IdentityResourceController(IIdentityResourceAppService identityResourceAppService)
    {
        IdentityResourceAppService = identityResourceAppService;
    }

    protected IIdentityResourceAppService IdentityResourceAppService { get; }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<IdentityResourceDto> GetAsync(Guid id)
    {
        return IdentityResourceAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<IdentityResourceDto>> GetListAsync(IdentityResourceGetListDto input)
    {
        return IdentityResourceAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<IdentityResourceDto> CreateAsync(IdentityResourceCreateInput input)
    {
        return IdentityResourceAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<IdentityResourceDto> UpdateAsync(Guid id, [FromBody] IdentityResourceUpdateInput input)
    {
        return IdentityResourceAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task<ListResultDto<IdentityResourceDto>> DeleteAsync([FromBody] List<Guid> ids)
    {
        return IdentityResourceAppService.DeleteAsync(ids);
    }

    [HttpGet]
    [Route("{id:guid}/claims")]
    public Task<ListResultDto<IdentityResourceClaimDto>> GetClaimsAsync(Guid id)
    {
        return IdentityResourceAppService.GetClaimsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/claims")]
    public Task AddClaimAsync(Guid id, [FromBody] IdentityResourceClaimCrateInput input)
    {
        return IdentityResourceAppService.AddClaimAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/claims")]
    public Task RemoveClaimAsync(Guid id, [FromBody] IdentityResourceClaimDeleteInput input)
    {
        return IdentityResourceAppService.RemoveClaimAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/properties")]
    public Task<ListResultDto<IdentityResourcePropertyDto>> GetPropertiesAsync(Guid id)
    {
        return IdentityResourceAppService.GetPropertiesAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/properties")]
    public Task AddPropertyAsync(Guid id, [FromBody] IdentityResourcePropertyCreateInput input)
    {
        return IdentityResourceAppService.AddPropertyAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/properties")]
    public Task RemovePropertyAsync(Guid id, [FromBody] IdentityResourcePropertyDeleteInput input)
    {
        return IdentityResourceAppService.RemovePropertyAsync(id, input);
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
        return IdentityResourceAppService.UpdateEnableAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public Task Disable(Guid id)
    {
        return IdentityResourceAppService.UpdateEnableAsync(id, false);
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
        return IdentityResourceAppService.UpdateShowInDiscoveryDocumentAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/hide")]
    public Task Hide(Guid id)
    {
        return IdentityResourceAppService.UpdateShowInDiscoveryDocumentAsync(id, false);
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
        return IdentityResourceAppService.UpdateEmphasizeAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/understate")]
    public Task Understate(Guid id)
    {
        return IdentityResourceAppService.UpdateEmphasizeAsync(id, false);
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
        return IdentityResourceAppService.UpdateRequiredAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/optional")]
    public Task Optional(Guid id)
    {
        return IdentityResourceAppService.UpdateRequiredAsync(id, false);
    }


}