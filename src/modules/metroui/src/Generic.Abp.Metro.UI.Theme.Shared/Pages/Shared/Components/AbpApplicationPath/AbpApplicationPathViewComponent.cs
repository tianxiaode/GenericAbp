using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Metro.UI.Theme.Shared.Pages.Shared.Components.AbpApplicationPath;

public class AbpApplicationPathViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        var applicationPath = ViewContext.HttpContext.Request.PathBase.Value;
        var model = new AbpApplicationPathViewComponentModel
        {
            ApplicationPath = applicationPath == null ? "/" : applicationPath.EnsureEndsWith('/')
        };

        return View("~/Pages/Shared/Components/AbpApplicationPath/Default.cshtml", model);
    }
}
