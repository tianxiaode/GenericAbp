using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public Task<PagedResultDto<ApiScopeDto>> GetListAsync()
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
}