using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Identity.Users;

[Area("Settings")]
[ControllerName("Settings")]
[Route("api/setting-management/password-policy")]
public class PasswordPolicyController : IdentityController, IPasswordPolicyAppService
{
    public PasswordPolicyController(IPasswordPolicyAppService passwordPolicyAppService)
    {
        PasswordPolicyAppService = passwordPolicyAppService;
    }

    protected IPasswordPolicyAppService PasswordPolicyAppService { get; }

    [HttpGet]
    public Task<PasswordPolicyDto> GetAsync()
    {
        return PasswordPolicyAppService.GetAsync();
    }

    [HttpPost]
    public Task UpdateAsync(PasswordPolicyUpdateDto input)
    {
        return PasswordPolicyAppService.UpdateAsync(input);
    }
}