using Generic.Abp.Identity.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace Generic.Abp.Identity.Settings;

[RemoteService(false)]
[Authorize(IdentityPermissions.IdentityManagement)]
public class IdentitySettingAppService : IdentityAppService, IIdentitySettingAppService
{
    public IdentitySettingAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    protected ISettingManager SettingManager { get; }

    public virtual async Task<IdentitySettingDto> GetAsync()
    {
        var dto = new IdentitySettingDto()
        {
            //密码设置
            RequiredLength = await GetSettingValueAsync<int>(IdentitySettingNames.Password.RequiredLength),
            RequiredUniqueChars = await GetSettingValueAsync<int>(IdentitySettingNames.Password.RequiredUniqueChars),
            RequireNonAlphanumeric =
                await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireNonAlphanumeric),
            RequireLowercase = await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireLowercase),
            RequireUppercase = await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireUppercase),
            RequireDigit = await GetSettingValueAsync<bool>(IdentitySettingNames.Password.RequireDigit),

            //密码更新设置
            ForceUsersToPeriodicallyChangePassword =
                await GetSettingValueAsync<bool>(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword),
            PasswordChangePeriodDays =
                await GetSettingValueAsync<int>(IdentitySettingNames.Password.PasswordChangePeriodDays),

            //锁定设置
            AllowedForNewUsers = await GetSettingValueAsync<bool>(IdentitySettingNames.Lockout.AllowedForNewUsers),
            LockoutDuration = await GetSettingValueAsync<int>(IdentitySettingNames.Lockout.LockoutDuration),
            MaxFailedAccessAttempts =
                await GetSettingValueAsync<int>(IdentitySettingNames.Lockout.MaxFailedAccessAttempts),

            //登录设置
            RequireConfirmedEmail =
                await GetSettingValueAsync<bool>(IdentitySettingNames.SignIn.RequireConfirmedEmail),
            EnablePhoneNumberConfirmation =
                await GetSettingValueAsync<bool>(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation),
            RequireConfirmedPhoneNumber =
                await GetSettingValueAsync<bool>(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber),

            //用户设置
            IsEmailUpdateEnabled = await GetSettingValueAsync<bool>(IdentitySettingNames.User.IsEmailUpdateEnabled),
            IsUserNameUpdateEnabled =
                await GetSettingValueAsync<bool>(IdentitySettingNames.User.IsUserNameUpdateEnabled),
        };
        return dto;
    }

    public virtual async Task UpdateAsync(IdentitySettingDto input)
    {
        await SetSettingValueAsync(IdentitySettingNames.Password.RequiredLength, input.RequiredLength);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequiredUniqueChars, input.RequiredUniqueChars);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, input.RequireNonAlphanumeric);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireLowercase, input.RequireLowercase);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireUppercase, input.RequireUppercase);
        await SetSettingValueAsync(IdentitySettingNames.Password.RequireDigit, input.RequireDigit);

        await SetSettingValueAsync(IdentitySettingNames.Password.ForceUsersToPeriodicallyChangePassword,
            input.ForceUsersToPeriodicallyChangePassword);
        await SetSettingValueAsync(IdentitySettingNames.Password.PasswordChangePeriodDays,
            input.PasswordChangePeriodDays);

        await SetSettingValueAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, input.AllowedForNewUsers);
        await SetSettingValueAsync(IdentitySettingNames.Lockout.LockoutDuration, input.LockoutDuration);
        await SetSettingValueAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, input.MaxFailedAccessAttempts);

        await SetSettingValueAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, input.RequireConfirmedEmail);
        await SetSettingValueAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation,
            input.EnablePhoneNumberConfirmation);
        await SetSettingValueAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber,
            input.RequireConfirmedPhoneNumber);

        await SetSettingValueAsync(IdentitySettingNames.User.IsEmailUpdateEnabled, input.IsEmailUpdateEnabled);
        await SetSettingValueAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled, input.IsUserNameUpdateEnabled);
    }

    protected virtual async Task<T> GetSettingValueAsync<T>(string name, T defaultValue = default(T))
        where T : struct
    {
        return await SettingProvider.GetAsync<T>(name);
    }

    protected virtual async Task SetSettingValueAsync<T>(string name, T value)
        where T : struct
    {
        await SettingManager.SetForCurrentTenantAsync(name, value.ToString());
    }
}