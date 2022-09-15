using Volo.Abp.Application.Services;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Application.Settings;

public interface IGenericAbpEmailSettingsAppService: IApplicationService
{
    Task<EmailSettingsDto> GetAsync();
    Task UpdateAsync(UpdateEmailSettingsDto input);
}