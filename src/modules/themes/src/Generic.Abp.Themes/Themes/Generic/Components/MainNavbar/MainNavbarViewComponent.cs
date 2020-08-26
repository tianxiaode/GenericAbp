using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Themes.Themes.Generic.Components.MainNavbar
{
    public class MainNavbarViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/Generic/Components/MainNavbar/Default.cshtml");
        }
    }
}
