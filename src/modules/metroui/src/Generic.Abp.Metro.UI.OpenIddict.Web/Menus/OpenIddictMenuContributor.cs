using Generic.Abp.OpenIddict.Localization;
using Generic.Abp.OpenIddict.Permissions;
using Volo.Abp.UI.Navigation;

namespace Generic.Abp.Metro.UI.OpenIddict.Web.Menus;

public class OpenIddictMenuContributor : IMenuContributor
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
        var l = context.GetLocalizer<OpenIddictResource>();


        var openIddictMenuItem = new ApplicationMenuItem(OpenIddictMenus.Prefix, l["Menu:OpenIddictManagement"]);
        openIddictMenuItem.AddItem(
            new ApplicationMenuItem(OpenIddictMenus.Applications, l["Applications"],
                url: "~/OpenIddict/Applications", requiredPermissionName: OpenIddictPermissions.Applications.Default));
        openIddictMenuItem.AddItem(
            new ApplicationMenuItem(OpenIddictMenus.Scopes, l["Scopes"], url: "~/OpenIddict/Scopes",
                requiredPermissionName: OpenIddictPermissions.Scopes.Default));

        context.Menu.GetAdministration().AddItem(openIddictMenuItem);

        return Task.CompletedTask;
    }
}