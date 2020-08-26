using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Themes.Themes.Generic.Components.Brand
{
    public class MainNavbarBrandViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/Generic/Components/Brand/Default.cshtml");
        }
    }
}
