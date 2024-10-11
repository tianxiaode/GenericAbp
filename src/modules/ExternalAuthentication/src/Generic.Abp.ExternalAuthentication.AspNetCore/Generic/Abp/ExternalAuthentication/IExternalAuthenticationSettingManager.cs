using Generic.Abp.ExternalAuthentication.dtos;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.ExternalAuthentication;

public interface IExternalAuthenticationSettingManager : ITransientDependency
{
    Task<ExternalSettingDto> GetSettingAsync();
    Task<List<ExternalProviderDto>> GetProvidersAsync();
    Task UpdateAsync(ExternalSettingUpdateDto input);
}