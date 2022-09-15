using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Application.Settings;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(EmailSettingsController), IncludeSelf = true)]
[RemoteService(false)]

public class AbandonEmailSettingsController: EmailSettingsController
{
    public AbandonEmailSettingsController(IEmailSettingsAppService emailSettingsAppService) : base(emailSettingsAppService)
    {
    }
}