using Generic.Abp.Themes.Shared.Toolbars;
using System.Threading.Tasks;

namespace Generic.Abp.Account.Web
{
    public class AccountModuleToolbarContributor : IToolbarContributor
    {
        public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
        {
            if (context.Toolbar.Name != StandardToolbars.Main)
            {
                return Task.CompletedTask;
            }

            //TODO: Currently disabled!
            //if (!context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
            //{
            //    context.Toolbar.Items.Add(new ToolbarItem(typeof(UserLoginLinkViewComponent)));
            //}

            return Task.CompletedTask;
        }
    }
}
