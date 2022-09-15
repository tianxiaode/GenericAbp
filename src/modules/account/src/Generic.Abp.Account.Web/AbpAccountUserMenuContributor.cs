using Generic.Abp.Account.Localization;
using Generic.Abp.Account.Web.Menus;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Generic.Abp.Account.Web
{
    public class AbpAccountUserMenuContributor : IMenuContributor
    {
        public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<AccountResource>();
            if (context.Menu.Name == StandardMenus.Main)
            {
                var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
                if (!currentUser.IsAuthenticated)
                {
                    context.Menu.Items.Insert(0, new ApplicationMenuItem(AccountMenus.Login, l["Login"], "~/Account/Login"));
                }

            }

            if (context.Menu.Name != StandardMenus.User)
            {
                return Task.CompletedTask;
            }

            var uiResource = context.GetLocalizer<AbpUiResource>();

            context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", l["ManageYourProfile"], url: "~/Account/Manage", icon: "fa fa-cog", order: 1000, null));
            context.Menu.AddItem(new ApplicationMenuItem("Account.Logout", uiResource["Logout"], url: "~/Account/Logout", icon: "fa fa-power-off", order: int.MaxValue - 1000));

            return Task.CompletedTask;
        }
    }
}
