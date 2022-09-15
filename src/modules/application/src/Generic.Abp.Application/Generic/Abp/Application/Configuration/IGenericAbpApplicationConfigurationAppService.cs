using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

namespace Generic.Abp.Application.Configuration;

public interface IGenericAbpApplicationConfigurationAppService : IApplicationService
{
    Task<ApplicationConfigurationDto> GetAsync();
    Task<ApplicationLocalizationConfigurationDto> GetLocalizationAsync();

}