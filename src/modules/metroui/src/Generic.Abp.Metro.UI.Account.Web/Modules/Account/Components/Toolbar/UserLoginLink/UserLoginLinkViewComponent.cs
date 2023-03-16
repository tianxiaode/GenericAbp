using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Account.Web.Modules.Account.Components.Toolbar.UserLoginLink;

public class UserLoginLinkViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Modules/Account/Components/Toolbar/UserLoginLink/Default.cshtml");
    }
}