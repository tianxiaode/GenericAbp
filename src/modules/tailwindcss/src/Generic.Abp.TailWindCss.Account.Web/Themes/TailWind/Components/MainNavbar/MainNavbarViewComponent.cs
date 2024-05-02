using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.TailWindCss.Account.Web.Themes.TailWind.Components.MainNavbar;

public class MainNavbarViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/TailWind/Components/MainNavbar/Default.cshtml");
    }
}