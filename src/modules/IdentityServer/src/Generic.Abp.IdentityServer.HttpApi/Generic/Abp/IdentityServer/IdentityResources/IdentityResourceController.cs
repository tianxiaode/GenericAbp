using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.IdentityServer.IdentityResources;

[Area("IdentityServer")]
[ControllerName("IdentityServer")]
[Route("api/identity-resources")]

public class IdentityResourceController: IdentityServerController, IIdentityResourceAppService
{
    public IdentityResourceController(IIdentityResourceAppService identityResourceAppService)
    {
        IdentityResourceAppService = identityResourceAppService;
    }

    protected IIdentityResourceAppService IdentityResourceAppService { get;  }

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
    public Task<IdentityResourceDto> UpdateAsync(Guid id,[FromBody] IdentityResourceUpdateInput input)
    {
        return IdentityResourceAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task<ListResultDto<IdentityResourceDto>> DeleteAsync([FromBody]List<Guid> ids)
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
}