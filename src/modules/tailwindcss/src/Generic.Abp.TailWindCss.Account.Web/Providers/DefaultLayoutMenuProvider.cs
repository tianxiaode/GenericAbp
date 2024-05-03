using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.TailWindCss.Account.Web.Providers;

public class DefaultLayoutMenuProvider : ILayoutMenuProvider, ITransientDependency
{
    protected virtual Type ResourceType { get; set; } = typeof(AccountResource);
    protected virtual List<LayoutMenu> MainMenus { get; set; } = [new LayoutMenu("Home", "~/")];
    protected virtual List<LayoutMenu> MobileMenus { get; set; } = [new LayoutMenu("Home", "~/")];

    public IServiceProvider ServiceProvider { get; }

    private readonly IAbpLazyServiceProvider _lazyServiceProvider;

    public IAuthorizationService AuthorizationService =>
        _lazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    public IStringLocalizerFactory StringLocalizerFactory =>
        _lazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();


    public DefaultLayoutMenuProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        _lazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();
    }

    public Task<bool> IsGrantedAsync(string policyName)
    {
        return AuthorizationService.IsGrantedAsync(policyName);
    }

    [CanBeNull]
    public IStringLocalizer GetDefaultLocalizer()
    {
        return StringLocalizerFactory.CreateDefaultOrNull();
    }

    [NotNull]
    public IStringLocalizer GetLocalizer<T>()
    {
        return StringLocalizerFactory.Create<T>();
    }

    [NotNull]
    public IStringLocalizer GetLocalizer(Type resourceType)
    {
        return StringLocalizerFactory.Create(resourceType);
    }

    public async Task<List<LayoutMenu>> GetMainMenusAsync()
    {
        return await GetMenusAsync(MainMenus);
    }

    public async Task<List<LayoutMenu>> GetMobileMenusAsync()
    {
        return await GetMenusAsync(MobileMenus);
    }

    private async Task<List<LayoutMenu>> GetMenusAsync(List<LayoutMenu> menus)
    {
        var l = GetLocalizer(ResourceType);
        var result = new List<LayoutMenu>();
        foreach (var menu in menus)
        {
            var name = l[menu.Name];
            if (menu.Permissions is { Length: > 0 } && await AuthorizationService.IsGrantedAnyAsync(menu.Permissions))
            {
                result.Add(new LayoutMenu(name, menu.Href));
            }
            else
            {
                result.Add(new LayoutMenu(name, menu.Href));
            }
        }

        return result;
    }
}