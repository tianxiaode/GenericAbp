using Generic.Abp.BusinessException.Exceptions;
using Generic.Abp.OAuthProviderManager.OAuthProviders.Dtos;
using Generic.Abp.OAuthProviderManager.Permissions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.OAuthProviderManager.OAuthProviders;

public class OAuthProviderAppService : OAuthProviderManagerAppService, IOAuthProviderAppService
{
    protected readonly IAuthenticationSchemeProvider SchemeProvider;
    protected readonly ISettingManager SettingManager;
    private const string SettingPrefix = "OAuthProvider.";


    public OAuthProviderAppService(IAuthenticationSchemeProvider schemeProvider, ISettingManager settingManager)
    {
        SchemeProvider = schemeProvider;
        SettingManager = settingManager;
    }

    public virtual async Task<ListResultDto<OAuthProviderDto>> GetListAsync(OAuthProviderGetListInput input)
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var result = new List<OAuthProviderDto>();
        foreach (var scheme in schemes)
        {
            var enabledString = await SettingManager.GetOrNullForCurrentTenantAsync($"{SettingPrefix}${scheme.Name}");
            var enabled = true;
            if (!enabledString.IsNullOrWhiteSpace())
            {
                enabled = enabledString == "true";
            }

            result.Add(new OAuthProviderDto()
            {
                Provider = scheme.Name,
                DisplayName = scheme.DisplayName,
                Enabled = enabled
            });
        }

        result = result.WhereIf(input.OnlyEnabled, m => m.Enabled).ToList();

        return new ListResultDto<OAuthProviderDto>(result);
    }

    [Authorize(OAuthProviderManagerPermissions.OAuthProviders.ManagePermissions)]
    public virtual async Task<OAuthProviderDto> UpdateAsync(string provider, OAuthProviderUpdateDto input)
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var scheme =
            schemes.FirstOrDefault(m => m.Name.Equals(provider, StringComparison.InvariantCultureIgnoreCase));
        if (scheme == null)
        {
            throw new EntityNotFoundBusinessException(L["OAuthProvider"], provider);
        }

        await SettingManager.SetForCurrentTenantAsync($"{SettingPrefix}${scheme.Name}", input.Enabled.ToString(), true);
        return new OAuthProviderDto()
        {
            Provider = scheme.Name,
            DisplayName = scheme.DisplayName,
            Enabled = input.Enabled
        };
    }
}