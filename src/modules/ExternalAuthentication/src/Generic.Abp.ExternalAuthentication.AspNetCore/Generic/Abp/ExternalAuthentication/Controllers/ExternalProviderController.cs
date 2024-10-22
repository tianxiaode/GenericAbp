using Generic.Abp.ExternalAuthentication.dtos;
using Generic.Abp.ExternalAuthentication.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExternalAuthentication.Controllers;

[Area("ExternalAuthenticationProviders")]
[Route("/api/external-providers")]
public class ExternalProviderController : AbpControllerBase
{
    protected IExternalAuthenticationSettingManager ExternalAuthenticationSettingManager { get; }

    public ExternalProviderController(IExternalAuthenticationSettingManager externalAuthenticationSettingManager)
    {
        ObjectMapperContext = typeof(GenericAbpExternalAuthenticationAspNetCoreModule);
        LocalizationResource = typeof(ExternalAuthenticationResource);
        ExternalAuthenticationSettingManager = externalAuthenticationSettingManager;
    }

    [HttpGet]
    public virtual async Task<ListResultDto<ExternalProviderDto>> GetListAsync()
    {
        var providers = await ExternalAuthenticationSettingManager.GetProvidersAsync();

        return new ListResultDto<ExternalProviderDto>(providers.Where(m => m.Enabled)
            .Select(p => new ExternalProviderDto(p.Provider, p.DisplayName)).ToList());
    }
}