using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace Generic.Abp.Application.Configuration;

[RemoteService(IsEnabled = false)]
public class GenericAbpApplicationConfigurationAppService : ApplicationService,
    IGenericAbpApplicationConfigurationAppService
{
    private readonly ILanguageProvider _languageProvider;
    private readonly AbpLocalizationOptions _localizationOptions;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    public GenericAbpApplicationConfigurationAppService(
        IOptions<AbpLocalizationOptions> localizationOptions,
        ILanguageProvider languageProvider,
        IPermissionDefinitionManager permissionDefinitionManager
    )
    {
        _localizationOptions = localizationOptions.Value;
        _languageProvider = languageProvider;
        _permissionDefinitionManager = permissionDefinitionManager;
    }


    public virtual async Task<ApplicationLocalizationConfigurationDto> GetLocalizationAsync()
    {
        var localizationConfig = new ApplicationLocalizationConfigurationDto();

        localizationConfig.Languages.AddRange(await _languageProvider.GetLanguagesAsync());

        if (_localizationOptions.DefaultResourceType != null)
        {
            localizationConfig.DefaultResourceName = LocalizationResourceNameAttribute.GetName(
                _localizationOptions.DefaultResourceType
            );
        }


        foreach (var resource in _localizationOptions.Resources.Values.WhereIf(!CurrentUser.IsAuthenticated,
                     x => x.ResourceName == "ExtResource" || x.ResourceName == localizationConfig.DefaultResourceName))
        {
            var dictionary = new Dictionary<string, string>();

            var localizer = await StringLocalizerFactory
                .CreateByResourceNameOrNullAsync(resource.ResourceName);

            if (localizer == null) continue;

            foreach (var localizedString in localizer.GetAllStrings())
            {
                dictionary[localizedString.Name] = localizedString.Value;
            }

            localizationConfig.Values[resource.ResourceName] = dictionary;
        }

        if (!CurrentUser.IsAuthenticated) return localizationConfig;

        var permissionResource = new Dictionary<string, string>();

        foreach (var group in await _permissionDefinitionManager.GetGroupsAsync())
        {
            permissionResource.Add(group.Name, group.DisplayName.Localize(StringLocalizerFactory));
            foreach (var permission in group.GetPermissionsWithChildren())
            {
                permissionResource.Add(permission.Name, permission.DisplayName.Localize(StringLocalizerFactory));
            }
        }

        localizationConfig.Values.Add("Permissions", permissionResource);


        return localizationConfig;
    }
}