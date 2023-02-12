using Generic.Abp.Metro.UI.Theme.Basic.Demo.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Generic.Abp.Metro.UI.Theme.Basic.Demo.Menu;

public class DemoMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<MetroUiThemeBasicDemoResource>();

        if (context.Menu.Name != StandardMenus.Main) return Task.CompletedTask;
        var menu = context.Menu.FindMenuItem(DemoMenuNames.Application.Main.Examples);
        if (menu == null)
        {
            menu = new ApplicationMenuItem(DemoMenuNames.Application.Main.Examples, l["Menu:Examples"]);
            context.Menu.AddItem(menu);
        }

        menu.AddItem(new ApplicationMenuItem(DemoMenuNames.Application.Main.ExamplesTagHelpers, l["TagHelpers"],
            url: "/tag-helpers"));
        return Task.CompletedTask;
    }
}