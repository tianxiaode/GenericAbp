using System.Threading.Tasks;
using Generic.Abp.Identity.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Generic.Abp.Identity.Users;

[RemoteService(false)]
[Authorize(IdentityPermissions.LookupPolicy)]
public class LookupPolicyAppService: IdentityAppService, ILookupPolicyAppService
{
        public LookupPolicyAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    private ISettingManager SettingManager { get; }
    public virtual async Task<LookupPolicyDto> GetAsync()
    {
        var dto = new LookupPolicyDto()
        {
            AllowedForNewUsers= await SettingProvider.GetAsync<bool>(IdentitySettingNames.Lockout.AllowedForNewUsers),
            LockoutDuration= await SettingProvider.GetAsync<int>(IdentitySettingNames.Lockout.LockoutDuration),
            MaxFailedAccessAttempts = await SettingProvider.GetAsync<int>(IdentitySettingNames.Lockout.MaxFailedAccessAttempts),
        };
        return dto;
    }

    public virtual async Task UpdateAsync(LookupPolicyUpdateDto input)
    {
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Lockout.LockoutDuration,
            input.LockoutDuration.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Lockout.AllowedForNewUsers,
            input.AllowedForNewUsers.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Lockout.MaxFailedAccessAttempts,
            input.MaxFailedAccessAttempts.ToString());
    }

}