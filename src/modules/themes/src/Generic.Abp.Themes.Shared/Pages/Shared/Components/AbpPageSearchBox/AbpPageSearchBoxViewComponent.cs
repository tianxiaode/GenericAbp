using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Themes.Shared.Pages.Shared.Components.AbpPageSearchBox
{
    public class AbpPageSearchBoxViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/Shared/Components/AbpPageSearchBox/Default.cshtml");
        }
    }
}
