using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;

namespace Pages.Abp.MultiTenancy;

[Area("abp")]
[RemoteService(Name = "abp")]
[Route("api/abp/multi-tenancy")]
public class TenantController : AbpControllerBase, IAbpTenantAppService
{
    private readonly IAbpTenantAppService _abpTenantAppService;

    public TenantController(IAbpTenantAppService abpTenantAppService)
    {
        _abpTenantAppService = abpTenantAppService;
    }

    [HttpGet]
    [Route("tenants/by-name/{name}")]
    public virtual async Task<FindTenantResultDto> FindTenantByNameAsync(string name)
    {
        return await _abpTenantAppService.FindTenantByNameAsync(name);
    }

    [HttpGet]
    [Route("tenants/by-id/{id}")]
    public virtual async Task<FindTenantResultDto> FindTenantByIdAsync(Guid id)
    {
        return await _abpTenantAppService.FindTenantByIdAsync(id);
    }
}
