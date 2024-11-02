using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Identity.Settings;

[Area("IdentitySettings")]
[Route("api/setting-management/identity")]
public class IdentitySettingController : IdentityController, IIdentitySettingAppService
{
    protected IIdentitySettingAppService IdentitySettingAppService { get; }

    public IdentitySettingController(IIdentitySettingAppService identitySettingAppService)
    {
        IdentitySettingAppService = identitySettingAppService;
    }

    [HttpGet]
    public Task<IdentitySettingDto> GetAsync()
    {
        return IdentitySettingAppService.GetAsync();
    }

    [HttpPut]
    public Task UpdateAsync([FromBody] IdentitySettingDto input)
    {
        return IdentitySettingAppService.UpdateAsync(input);
    }
}