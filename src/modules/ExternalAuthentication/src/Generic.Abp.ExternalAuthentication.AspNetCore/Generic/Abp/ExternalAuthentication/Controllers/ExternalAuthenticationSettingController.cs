using Generic.Abp.ExternalAuthentication.dtos;
using Generic.Abp.ExternalAuthentication.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.ExternalAuthentication.Controllers;

[Area("ExternalAuthenticationProviders")]
[Route("/api/external-authentication-settings")]
public class ExternalAuthenticationSettingController(IExternalAuthenticationSettingManager externalSettingManager)
    : AbpController
{
    protected IExternalAuthenticationSettingManager ExternalSettingManager { get; } = externalSettingManager;


    [HttpGet]
    public virtual async Task<ExternalSettingDto> GetAsync()
    {
        return await ExternalSettingManager.GetSettingAsync();
    }

    [Authorize(ExternalAuthenticationPermissions.ExternalAuthenticationProviders.ManagePermissions)]
    [HttpPut]
    public virtual async Task<ExternalSettingDto> UpdateAsync(ExternalSettingUpdateDto input)
    {
        await ExternalSettingManager.UpdateAsync(input);
        return await ExternalSettingManager.GetSettingAsync();
    }
}