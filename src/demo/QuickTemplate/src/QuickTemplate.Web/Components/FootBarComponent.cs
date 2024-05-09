using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace QuickTemplate.Web.Components;

public class FootBarComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("/Components/FootBar.cshtml");
    }
}