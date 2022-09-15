using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Emailing;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Application.Settings;

[Authorize(SettingManagementPermissions.Emailing)]
public class GenericAbpEmailSettingsAppService: ApplicationService,IGenericAbpEmailSettingsAppService
{
    protected ISettingManager SettingManager { get; }

    public GenericAbpEmailSettingsAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    public virtual async Task<EmailSettingsDto> GetAsync()
    {
        await CheckFeatureAsync();

        var settingsDto = new EmailSettingsDto
        {
            SmtpHost = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Host),
            SmtpPort = Convert.ToInt32(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Port)),
            SmtpUserName = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.UserName),
            SmtpPassword = null,
            SmtpDomain = await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Domain),
            SmtpEnableSsl = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.EnableSsl)),
            SmtpUseDefaultCredentials = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(EmailSettingNames.Smtp.UseDefaultCredentials)),
            DefaultFromAddress = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromAddress),
            DefaultFromDisplayName = await SettingProvider.GetOrNullAsync(EmailSettingNames.DefaultFromDisplayName),
        };

        if (CurrentTenant.IsAvailable)
        {
            settingsDto.SmtpHost = await SettingManager.GetOrNullForTenantAsync(EmailSettingNames.Smtp.Host, CurrentTenant.GetId(), false);
            settingsDto.SmtpUserName = await SettingManager.GetOrNullForTenantAsync(EmailSettingNames.Smtp.UserName, CurrentTenant.GetId(), false);
            settingsDto.SmtpPassword = null;
            settingsDto.SmtpDomain = await SettingManager.GetOrNullForTenantAsync(EmailSettingNames.Smtp.Domain, CurrentTenant.GetId(), false);
        }

        return settingsDto;
    }

    public virtual async Task UpdateAsync(UpdateEmailSettingsDto input)
    {
        await CheckFeatureAsync();

        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Host, input.SmtpHost);
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Port, input.SmtpPort.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.UserName, input.SmtpUserName);
        if (!string.IsNullOrEmpty(input.SmtpPassword))
        {
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Password, input.SmtpPassword);
        }
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.Domain, input.SmtpDomain);
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
        await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
    }

    protected virtual async Task CheckFeatureAsync()
    {
        await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        if (CurrentTenant.IsAvailable)
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.AllowTenantsToChangeEmailSettings);
        }
    }
}