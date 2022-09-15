using System.Threading.Tasks;
using Generic.Abp.Identity.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Generic.Abp.Identity.Users;

[RemoteService(false)]
[Authorize(IdentityPermissions.PasswordPolicy)]

public class PasswordPolicyAppService: ApplicationService, IPasswordPolicyAppService
{
    public PasswordPolicyAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    private ISettingManager SettingManager { get; }
    public virtual async Task<PasswordPolicyDto> GetAsync()
    {
        var dto = new PasswordPolicyDto()
        {
            RequiredLength = await SettingProvider.GetAsync<int>(IdentitySettingNames.Password.RequiredLength),
            RequireDigit = await SettingProvider.GetAsync<bool>(IdentitySettingNames.Password.RequireDigit),
            //RequiredUniqueChars = await SettingProvider.GetAsync<int>(IdentitySettingNames.Password.RequiredUniqueChars),
            RequireLowercase = await SettingProvider.GetAsync<bool>(IdentitySettingNames.Password.RequireLowercase),
            RequireNonAlphanumeric = await SettingProvider.GetAsync<bool>(IdentitySettingNames.Password.RequireNonAlphanumeric),
            RequireUppercase = await SettingProvider.GetAsync<bool>(IdentitySettingNames.Password.RequireUppercase),
        };
        return dto;
    }

    public virtual async Task UpdateAsync(PasswordPolicyUpdateDto input)
    {
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequiredLength,
            input.RequiredLength.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireDigit,
            input.RequireDigit.ToString());
        // await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequiredUniqueChars,
        //     input.RequiredUniqueChars.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireLowercase,
            input.RequireLowercase.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireNonAlphanumeric,
            input.RequireNonAlphanumeric.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireUppercase,
            input.RequireUppercase.ToString());
    }
}