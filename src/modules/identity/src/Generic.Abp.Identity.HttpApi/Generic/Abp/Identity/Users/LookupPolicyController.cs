using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Identity.Users;

[Area("Settings")]
[ControllerName("Settings")]
[Route("api/setting-management/lookup-policy")]
public class LookupPolicyController : IdentityController, ILookupPolicyAppService
{
    public LookupPolicyController(ILookupPolicyAppService lookupPolicyAppService)
    {
        LookupPolicyAppService = lookupPolicyAppService;
    }

    protected ILookupPolicyAppService LookupPolicyAppService { get; }

    [HttpGet]
    public Task<LookupPolicyDto> GetAsync()
    {
        return LookupPolicyAppService.GetAsync();
    }

    [HttpPost]
    public Task UpdateAsync(LookupPolicyUpdateDto input)
    {
        return LookupPolicyAppService.UpdateAsync(input);
    }
}