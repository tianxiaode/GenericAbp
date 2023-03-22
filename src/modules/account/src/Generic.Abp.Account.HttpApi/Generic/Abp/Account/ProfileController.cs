using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Account;

[RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
[Area(AccountRemoteServiceConsts.ModuleName)]
[ControllerName("Profile")]
[Route("/api/account/my-profile")]
public class ProfileController : AbpControllerBase, IProfileAppService
{
    protected IProfileAppService ProfileAppService { get; }

    public ProfileController(IProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;
    }

    [HttpGet]
    public virtual Task<ProfileDto> GetAsync()
    {
        return ProfileAppService.GetAsync();
    }

    [HttpPut]
    public virtual Task<ProfileDto> UpdateAsync([FromBody] UpdateProfileDto input)
    {
        Logger.LogInformation($"UpdateProfileDto: {System.Text.Json.JsonSerializer.Serialize(input)}");
        return ProfileAppService.UpdateAsync(input);
    }

    [HttpPost]
    [Route("change-password")]
    public virtual Task ChangePasswordAsync([FromBody] ChangePasswordInput input)
    {
        return ProfileAppService.ChangePasswordAsync(input);
    }
}