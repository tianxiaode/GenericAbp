using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.IdentityServer.ApiResources;

[Area("IdentityServer")]
[ControllerName("IdentityServer")]
[Route("api/api-resources")]

public class ApiResourceController: IdentityServerController, IApiResourceAppService
{
    public ApiResourceController(IApiResourceAppService apiResourceAppService)
    {
        ApiResourceAppService = apiResourceAppService;
    }

    protected IApiResourceAppService ApiResourceAppService { get;  }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<ApiResourceDto> GetAsync(Guid id)
    {
        return ApiResourceAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<ApiResourceDto>> GetListAsync(ApiResourceGetListDto input)
    {
        return ApiResourceAppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<ApiResourceDto> CreateAsync(ApiResourceCreateInput input)
    {
        return ApiResourceAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<ApiResourceDto> UpdateAsync(Guid id,[FromBody] ApiResourceUpdateInput input)
    {
        return ApiResourceAppService.UpdateAsync(id, input);
    }

    [RemoteService(false)]
    public Task UpdateEnableAsync(Guid id, bool enable)
    {
        return Task.CompletedTask;
    }

    [HttpPut]
    [Route("{id:guid}/enable")]
    public Task Enable(Guid id)
    {
        return ApiResourceAppService.UpdateEnableAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/disable")]
    public Task Disable(Guid id)
    {
        return ApiResourceAppService.UpdateEnableAsync(id, false);
    }

    [RemoteService(false)]
    public Task UpdateShowInDiscoveryDocumentAsync(Guid id, bool enable)
    {
        return Task.CompletedTask;
    }

    [HttpPut]
    [Route("{id:guid}/show")]
    public Task Show(Guid id)
    {
        return ApiResourceAppService.UpdateShowInDiscoveryDocumentAsync(id, true);
    }

    [HttpPut]
    [Route("{id:guid}/hide")]
    public Task Hide(Guid id)
    {
        return ApiResourceAppService.UpdateShowInDiscoveryDocumentAsync(id, false);
    }

    [HttpDelete]
    public Task<ListResultDto<ApiResourceDto>> DeleteAsync([FromBody]List<Guid> ids)
    {
        return ApiResourceAppService.DeleteAsync(ids);
    }

    [HttpGet]
    [Route("{id:guid}/claims")]
    public Task<ListResultDto<ApiResourceClaimDto>> GetClaimsAsync(Guid id)
    {
        return ApiResourceAppService.GetClaimsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/claims")]
    public Task AddClaimAsync(Guid id, [FromBody] ApiResourceClaimCreateInput input)
    {
        return ApiResourceAppService.AddClaimAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/claims")]
    public Task RemoveClaimAsync(Guid id, [FromBody] ApiResourceClaimDeleteInput input)
    {
        return ApiResourceAppService.RemoveClaimAsync(id, input);
    }

    [HttpGet]
    [Route("{id:guid}/secrets")]
    public Task<ListResultDto<ApiResourceSecretDto>> GetClientSecretsAsync(Guid id)
    {
        return ApiResourceAppService.GetClientSecretsAsync(id);
    }

    [HttpPut]
    [Route("{id:guid}/secrets")]
    public Task AddSecretAsync(Guid id, [FromBody] ApiResourceSecretCreateInput input)
    {
        return ApiResourceAppService.AddSecretAsync(id, input);
    }

    [HttpDelete]
    [Route("{id:guid}/secrets")]
    public Task RemoveSecretAsync(Guid id, [FromBody] ApiResourceSecretDeleteInput input)
    {
        return ApiResourceAppService.RemoveSecretAsync(id, input);
    }
}