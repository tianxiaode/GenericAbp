using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Emailing;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Application.Settings;

[Area("Settings")]
[ControllerName("Settings")]
[Route("api/settings/emailing")]

public class GenericAbpEmailSettingsController:AbpControllerBase,IGenericAbpEmailSettingsAppService
{
    public GenericAbpEmailSettingsController(IEmailSender emailSender, IGenericAbpEmailSettingsAppService genericAbpEmailSettingsAppService)
    {
        EmailSender = emailSender;
        GenericAbpEmailSettingsAppService = genericAbpEmailSettingsAppService;
    }

    protected IGenericAbpEmailSettingsAppService GenericAbpEmailSettingsAppService { get; }

    [HttpGet]
    public Task<EmailSettingsDto> GetAsync()
    {
        return GenericAbpEmailSettingsAppService.GetAsync();
    }

    [HttpPost]
    public Task UpdateAsync(UpdateEmailSettingsDto input)
    {
        return GenericAbpEmailSettingsAppService.UpdateAsync(input);
    }

    protected IEmailSender EmailSender { get; }
    [HttpPost]
    [Route("send-test")]
    public virtual Task SendTestAsync(SendTestDto input)
    {
        return EmailSender.SendAsync(input.To, input.Subject, input.Body);
    }
}