using Generic.Abp.IdentityServer.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.UI.Navigation;

namespace Generic.Abp.IdentityServer.Web.Navigation;

public class IdentityServerWebMainMenuContributor: IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<AbpIdentityServerResource>();

        var identityServerMenuItem = new ApplicationMenuItem(IdentityServerMenuNames.GroupName, l["Menu:IdentityServerManagement"]);
        identityServerMenuItem.AddItem(
            new ApplicationMenuItem(IdentityServerMenuNames.ApiResources, l["ApiResources"],
                url: "~/IdentityServer/ApiResources").RequirePermissions(IdentityServerPermissions.ApiResources.Default));
        identityServerMenuItem.AddItem(
            new ApplicationMenuItem(IdentityServerMenuNames.Clients, l["Clients"], url: "~/IdentityServer/Clients")
                .RequirePermissions(IdentityServerPermissions.Clients.Default));
        identityServerMenuItem.AddItem(
            new ApplicationMenuItem(IdentityServerMenuNames.IdentityResources, l["IdentityResources"],
                    url: "~/IdentityServer/IdentityResources")
                .RequirePermissions(IdentityServerPermissions.IdentityResources.Default));

        context.Menu.GetAdministration().AddItem(identityServerMenuItem);

        return Task.CompletedTask;
    }
}