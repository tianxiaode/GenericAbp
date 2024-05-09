using QuickTemplate.Localization;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.Identity.Web.Navigation;
using Generic.Abp.Metro.UI.Theme.Basic.Demo.Menu;
using Volo.Abp.UI.Navigation;

namespace QuickTemplate.Web.Menus;

public class QuickTemplateMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var demoMenu = context.Menu.FindMenuItem(DemoMenuNames.Application.Main.Examples);

        var l = context.GetLocalizer<QuickTemplateResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                QuickTemplateMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

        //context.Menu.Items.Add(new ApplicationMenuItem(
        //    QuickTemplateMenus.About,
        //    l["Menu:About"],
        //    "~/About"
        //));

        context.Menu.Items.Add(new ApplicationMenuItem(
            QuickTemplateMenus.Github,
            "Github",
            "https://github.com/tianxiaode/QuickTemplate",
            "fab fa-github"
        ));

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        //administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

        demoMenu.AddItem(new ApplicationMenuItem("DesktopClient", l["DesktopClient"], "https://d.extjs.tech"));
        demoMenu.AddItem(new ApplicationMenuItem("MobileClient", l["MobileClient"], "https://m.extjs.tech"));
        demoMenu.AddItem(new ApplicationMenuItem("Swagger", "Swagger", "/Swagger"));

        return Task.CompletedTask;
    }
}