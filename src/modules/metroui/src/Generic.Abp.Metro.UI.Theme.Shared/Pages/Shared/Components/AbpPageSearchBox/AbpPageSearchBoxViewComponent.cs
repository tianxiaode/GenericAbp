using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Theme.Shared.Pages.Shared.Components.AbpPageSearchBox;

public class AbpPageSearchBoxViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Pages/Shared/Components/AbpPageSearchBox/Default.cshtml");
    }
}
