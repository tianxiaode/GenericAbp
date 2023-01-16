using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Theme.Shared.Toolbars;

public interface IToolbarConfigurationContext : IServiceProviderAccessor
{
    ITheme Theme { get; }

    Toolbar Toolbar { get; }

    IAuthorizationService AuthorizationService { get; }

    IStringLocalizerFactory StringLocalizerFactory { get; }

    Task<bool> IsGrantedAsync(string policyName);

    [CanBeNull]
    IStringLocalizer GetDefaultLocalizer();

    [NotNull]
    public IStringLocalizer GetLocalizer<T>();

    [NotNull]
    public IStringLocalizer GetLocalizer(Type resourceType);
}
