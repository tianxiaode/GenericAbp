﻿using Generic.Abp.Demo.Localization;
using System.Threading.Tasks;
using Generic.Abp.Demo.MultiTenancy;
using Generic.Abp.Metro.UI.Identity.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace Generic.Abp.Demo.Web.Menus
{
    public class DemoMenuContributor : IMenuContributor
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
            var l = context.GetLocalizer<DemoResource>();

            context.Menu.Items.Insert(
                0,
                new ApplicationMenuItem(
                    DemoMenus.Home,
                    l["Menu:Home"],
                    "~/",
                    icon: "mif-home",
                    order: 0
                )
            );

            if (MultiTenancyConsts.IsEnabled)
            {
                //administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                //administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            //administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);

            return Task.CompletedTask;
        }
    }
}