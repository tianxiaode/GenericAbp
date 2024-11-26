using Generic.Abp.ExternalAuthentication.dtos;
using Generic.Abp.ExternalAuthentication.Localization;
using Generic.Abp.ExternalAuthentication.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.ExternalAuthentication;

public class ExternalAuthenticationSettingManager : IExternalAuthenticationSettingManager
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    protected IStringLocalizerFactory StringLocalizerFactory =>
        LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

    protected IStringLocalizer L
    {
        get { return _localizer ??= CreateLocalizer(); }
    }

    private IStringLocalizer? _localizer;

    protected Type? LocalizationResource
    {
        get => _localizationResource;
        set
        {
            _localizationResource = value;
            _localizer = null;
        }
    }

    private Type? _localizationResource = typeof(DefaultResource);


    protected ISettingManager SettingManager { get; }
    protected IAuthenticationSchemeProvider SchemeProvider { get; }

    public ExternalAuthenticationSettingManager(ISettingManager settingManager,
        IAuthenticationSchemeProvider schemeProvider)
    {
        SettingManager = settingManager;
        SchemeProvider = schemeProvider;
        LocalizationResource = typeof(ExternalAuthenticationResource);
    }

    public virtual async Task<ExternalSettingDto> GetSettingAsync()
    {
        var dto = new ExternalSettingDto
        {
            NewUserPrefix = await SettingManager.GetOrNullForCurrentTenantAsync(
                ExternalAuthenticationSettingNames.NewUser.NewUserPrefix) ?? "",
            NewUserEmailSuffix = await SettingManager.GetOrNullForCurrentTenantAsync(
                ExternalAuthenticationSettingNames.NewUser.NewUserEmailSuffix) ?? "",
            Providers = await GetProvidersAsync()
        };
        return dto;
    }

    public virtual async Task<List<ExternalProviderDto>> GetProvidersAsync()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        var result = new List<ExternalProviderDto>();
        foreach (var scheme in schemes.Where(x => x.DisplayName != null))
        {
            var providerString = await SettingManager.GetOrNullForCurrentTenantAsync(
                ExternalAuthenticationSettingNames.Provider.ProviderPrefix + scheme.Name, false);
            var provider = string.IsNullOrEmpty(providerString)
                ? new ExternalProviderDto(scheme.Name, L[scheme.Name] ?? "", "", "", false)
                : System.Text.Json.JsonSerializer.Deserialize<ExternalProviderDto>(providerString) ??
                  new ExternalProviderDto(scheme.Name, L[scheme.Name] ?? "", "", "", false);

            result.Add(provider);
        }

        return result;
    }

    public virtual async Task<ExternalProviderDto> GetProviderAsync(string scheme)
    {
        var providerString = await SettingManager.GetOrNullForCurrentTenantAsync(
            ExternalAuthenticationSettingNames.Provider.ProviderPrefix + scheme, false);

        var provider = string.IsNullOrEmpty(providerString)
            ? new ExternalProviderDto(scheme, L[scheme] ?? "", "", "", false)
            : System.Text.Json.JsonSerializer.Deserialize<ExternalProviderDto>(providerString) ??
              new ExternalProviderDto(scheme, L[scheme] ?? "", "", "", false);

        return provider;
    }

    public virtual async Task UpdateAsync(ExternalSettingUpdateDto input)
    {
        await SettingManager.SetForCurrentTenantAsync(
            ExternalAuthenticationSettingNames.NewUser.NewUserPrefix, input.NewUserPrefix);
        await SettingManager.SetForCurrentTenantAsync(
            ExternalAuthenticationSettingNames.NewUser.NewUserEmailSuffix, input.NewUserEmailSuffix);

        var schemes = await SchemeProvider.GetAllSchemesAsync();
        foreach (var scheme in schemes.Where(x => x.DisplayName != null))
        {
            var provider = input.Providers.FirstOrDefault(x => x.Provider == scheme.Name);
            if (provider == null)
            {
                continue;
            }

            await SettingManager.SetForCurrentTenantAsync(
                ExternalAuthenticationSettingNames.Provider.ProviderPrefix + scheme.Name,
                System.Text.Json.JsonSerializer.Serialize(provider));
        }
    }


    protected virtual IStringLocalizer CreateLocalizer()
    {
        if (LocalizationResource != null)
        {
            return StringLocalizerFactory.Create(LocalizationResource);
        }

        var localizer = StringLocalizerFactory.CreateDefaultOrNull();
        if (localizer == null)
        {
            throw new AbpException(
                $"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
        }

        return localizer;
    }
}