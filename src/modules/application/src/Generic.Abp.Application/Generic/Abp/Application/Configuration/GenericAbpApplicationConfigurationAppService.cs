using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
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
using TimeZone = Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.TimeZone;

namespace Generic.Abp.Application.Configuration;

[RemoteService(IsEnabled = false)]
public class GenericAbpApplicationConfigurationAppService : ApplicationService, IGenericAbpApplicationConfigurationAppService
{
    private readonly ILanguageProvider _languageProvider;
    private readonly AbpLocalizationOptions _localizationOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly IAbpAuthorizationPolicyProvider _abpAuthorizationPolicyProvider;
    private readonly IFeatureDefinitionManager _featureDefinitionManager;
    private readonly ICurrentUser _currentUser;
    private readonly ISettingDefinitionManager _settingDefinitionManager;
    private readonly ISettingProvider _settingProvider;
    private readonly ITimezoneProvider _timezoneProvider;
    private readonly AbpClockOptions _abpClockOptions;
    private readonly ICachedObjectExtensionsDtoService _cachedObjectExtensionsDtoService;
    private readonly IPermissionDefinitionManager _permissionDefinitionManager;

    public GenericAbpApplicationConfigurationAppService(
        IServiceProvider serviceProvider,
        IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
        IFeatureDefinitionManager featureDefinitionManager,
        IOptions<AbpLocalizationOptions> localizationOptions,
        ILanguageProvider languageProvider,
        ICurrentUser currentUser,
        ISettingDefinitionManager settingDefinitionManager,
        ISettingProvider settingProvider,
        ITimezoneProvider timezoneProvider,
        IOptions<AbpClockOptions> abpClockOptions,
        ICachedObjectExtensionsDtoService cachedObjectExtensionsDtoService,
        IPermissionDefinitionManager permissionDefinitionManager
    )
    {
        _serviceProvider = serviceProvider;
        _localizationOptions = localizationOptions.Value;
        _languageProvider = languageProvider;
        _abpAuthorizationPolicyProvider = abpAuthorizationPolicyProvider;
        _featureDefinitionManager = featureDefinitionManager;
        _currentUser = currentUser;
        _settingDefinitionManager = settingDefinitionManager;
        _settingProvider = settingProvider;
        _timezoneProvider = timezoneProvider;
        _abpClockOptions = abpClockOptions.Value;
        _cachedObjectExtensionsDtoService = cachedObjectExtensionsDtoService;
        _permissionDefinitionManager = permissionDefinitionManager;
    }

    public virtual async Task<ApplicationConfigurationDto> GetAsync()
    {
        Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetAsync()...");

        var result = new ApplicationConfigurationDto
        {
            //Localization = await GetLocalizationAsync(),
            Auth = await GetAuthConfigAsync(),
            Features = await GetFeaturesConfigAsync(),
            CurrentUser = GetCurrentUser(),
            Setting = await GetSettingConfigAsync(),
            Timing = await GetTimingConfigAsync(),
            Clock = GetClockConfig(),
            ObjectExtensions = _cachedObjectExtensionsDtoService.Get()
        };

        Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetAsync().");


        return result;
    }

    public virtual async Task<ApplicationLocalizationConfigurationDto> GetLocalizationAsync()
    {
        var localizationConfig = new ApplicationLocalizationConfigurationDto();

        localizationConfig.Languages.AddRange(await _languageProvider.GetLanguagesAsync());

        foreach (var resource in _localizationOptions.Resources.Values)
        {
            if (!CurrentUser.IsAuthenticated &&
                resource.ResourceName != "ExtResource")
            {
                continue;
            }

            var dictionary = new Dictionary<string, string>();

            var localizer = _serviceProvider.GetRequiredService(
                typeof(IStringLocalizer<>).MakeGenericType(resource.ResourceType)
            ) as IStringLocalizer;

            if (localizer == null) continue;

            foreach (var localizedString in localizer.GetAllStrings())
            {
                dictionary[localizedString.Name] = localizedString.Value;
            }

            localizationConfig.Values[resource.ResourceName] = dictionary;
        }


        var permissionResource = new Dictionary<string, string>();

        foreach (var group in _permissionDefinitionManager.GetGroups())
        {
            permissionResource.Add(group.Name, group.DisplayName.Localize(StringLocalizerFactory));
            foreach (var permission in group.GetPermissionsWithChildren())
            {
                permissionResource.Add(permission.Name, permission.DisplayName.Localize(StringLocalizerFactory));
            }

        }

        localizationConfig.Values.Add("Permissions", permissionResource);


        localizationConfig.CurrentCulture = GetCurrentCultureInfo();

        if (_localizationOptions.DefaultResourceType != null)
        {
            localizationConfig.DefaultResourceName = LocalizationResourceNameAttribute.GetName(
                _localizationOptions.DefaultResourceType
            );
        }

        localizationConfig.LanguagesMap = _localizationOptions.LanguagesMap;
        localizationConfig.LanguageFilesMap = _localizationOptions.LanguageFilesMap;

        return localizationConfig;
    }



    protected virtual ClockDto GetClockConfig()
    {
        return new ClockDto
        {
            Kind = Enum.GetName(typeof(DateTimeKind), _abpClockOptions.Kind)
        };
    }

    protected virtual async Task<TimingDto> GetTimingConfigAsync()
    {
        var windowsTimeZoneId = await _settingProvider.GetOrNullAsync(TimingSettingNames.TimeZone);

        return new TimingDto
        {
            TimeZone = new TimeZone
            {
                Windows = new WindowsTimeZone
                {
                    TimeZoneId = windowsTimeZoneId
                },
                Iana = new IanaTimeZone
                {
                    TimeZoneName = windowsTimeZoneId.IsNullOrWhiteSpace()
                        ? null
                        : _timezoneProvider.WindowsToIana(windowsTimeZoneId)
                }
            }
        };
    }

    protected virtual async Task<ApplicationSettingConfigurationDto> GetSettingConfigAsync()
    {
        var result = new ApplicationSettingConfigurationDto
        {
            Values = new Dictionary<string, string>()
        };

        foreach (var settingDefinition in _settingDefinitionManager.GetAll())
        {
            if (!settingDefinition.IsVisibleToClients)
            {
                continue;
            }

            result.Values[settingDefinition.Name] = await _settingProvider.GetOrNullAsync(settingDefinition.Name);
        }

        return result;
    }

    protected virtual async Task<ApplicationFeatureConfigurationDto> GetFeaturesConfigAsync()
    {
        var result = new ApplicationFeatureConfigurationDto();

        foreach (var featureDefinition in _featureDefinitionManager.GetAll())
        {
            if (!featureDefinition.IsVisibleToClients)
            {
                continue;
            }

            result.Values[featureDefinition.Name] = await FeatureChecker.GetOrNullAsync(featureDefinition.Name);
        }

        return result;
    }

    protected virtual async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
    {
        var authConfig = new ApplicationAuthConfigurationDto();

        var policyNames = await _abpAuthorizationPolicyProvider.GetPoliciesNamesAsync();

        foreach (var policyName in policyNames)
        {
            authConfig.Policies[policyName] = true;

            if (await AuthorizationService.IsGrantedAsync(policyName))
            {
                authConfig.GrantedPolicies[policyName] = true;
            }
        }

        return authConfig;
    }

    protected virtual CurrentUserDto GetCurrentUser()
    {
        return new CurrentUserDto
        {
            IsAuthenticated = _currentUser.IsAuthenticated,
            Id = _currentUser.Id,
            TenantId = _currentUser.TenantId,
            UserName = _currentUser.UserName,
            Email = _currentUser.Email,
            Roles = _currentUser.Roles
        };
    }


    private static CurrentCultureDto GetCurrentCultureInfo()
    {
        return new CurrentCultureDto
        {
            Name = CultureInfo.CurrentUICulture.Name,
            DisplayName = CultureInfo.CurrentUICulture.DisplayName,
            EnglishName = CultureInfo.CurrentUICulture.EnglishName,
            NativeName = CultureInfo.CurrentUICulture.NativeName,
            IsRightToLeft = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
            CultureName = CultureInfo.CurrentUICulture.TextInfo.CultureName,
            TwoLetterIsoLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            ThreeLetterIsoLanguageName = CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName,
            DateTimeFormat = new DateTimeFormatDto
            {
                CalendarAlgorithmType =
                    CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType.ToString(),
                DateTimeFormatLong = CultureInfo.CurrentUICulture.DateTimeFormat.LongDatePattern,
                ShortDatePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                FullDateTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern,
                DateSeparator = CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator,
                ShortTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern,
                LongTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern,
            }
        };
    }


}