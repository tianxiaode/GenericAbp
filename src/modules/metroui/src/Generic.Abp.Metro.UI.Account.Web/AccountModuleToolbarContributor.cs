using System.Threading.Tasks;
using Generic.Abp.Metro.UI.Account.Web.Modules.Account.Components.Toolbar.UserLoginLink;
using Generic.Abp.Metro.UI.Theme.Shared.Toolbars;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;

namespace Generic.Abp.Metro.UI.Account.Web;

public class AccountModuleToolbarContributor : IToolbarContributor
{
    public virtual Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return Task.CompletedTask;
        }

        if (!context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(UserLoginLinkViewComponent)));
        }

        return Task.CompletedTask;
    }
}