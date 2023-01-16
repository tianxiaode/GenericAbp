using System.Threading.Tasks;
using Generic.Abp.Metro.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch;
using Generic.Abp.Metro.UI.Theme.Basic.Themes.Basic.Components.Toolbar.UserMenu;
using Generic.Abp.Metro.UI.Theme.Shared.Toolbars;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace Generic.Abp.Metro.UI.Theme.Basic.Toolbars;

public class BasicThemeMainTopToolbarContributor : IToolbarContributor
{
    public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return;
        }

        if (context.Theme is not BasicTheme)
        {
            return;
        }

        var languageProvider = context.ServiceProvider.GetService<ILanguageProvider>();

        //TODO: This duplicates GetLanguages() usage. Can we eleminate this?
        var languages = await languageProvider?.GetLanguagesAsync()!;
        if (languages.Count > 1)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchViewComponent)));
        }

        if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(UserMenuViewComponent)));
        }
    }
}
