using Generic.Abp.ExternalAuthentication.dtos;
using Generic.Abp.ExternalAuthentication.Localization;
using Generic.Abp.ExternalAuthentication.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExternalAuthentication.Controllers;

[Area("ExternalAuthenticationProviders")]
[Route("/api/setting-management/external-authentication")]
public class ExternalAuthenticationSettingController
    : AbpController
{
    public ExternalAuthenticationSettingController(IExternalAuthenticationSettingManager externalSettingManager)
    {
        ObjectMapperContext = typeof(GenericAbpExternalAuthenticationAspNetCoreModule);
        LocalizationResource = typeof(ExternalAuthenticationResource);
        ExternalSettingManager = externalSettingManager;
    }

    protected IExternalAuthenticationSettingManager ExternalSettingManager { get; }


    [HttpGet]
    [Authorize(ExternalAuthenticationPermissions.ExternalAuthenticationManagement)]
    public virtual async Task<ExternalSettingDto> GetAsync()
    {
        return await ExternalSettingManager.GetSettingAsync();
    }

    [Authorize(ExternalAuthenticationPermissions.ExternalAuthenticationManagement)]
    [HttpPut]
    public virtual async Task<ExternalSettingDto> UpdateAsync([FromBody] ExternalSettingUpdateDto input)
    {
        await ExternalSettingManager.UpdateAsync(input);
        return await ExternalSettingManager.GetSettingAsync();
    }
}