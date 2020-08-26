using Microsoft.AspNetCore.Mvc;
using System;
using Volo.Abp.AspNetCore.Mvc;

namespace Generic.Abp.Themes.Shared.Pages.Shared.Components.AbpApplicationPath
{
    public class AbpApplicationPathViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var applicationPath = ViewContext.HttpContext.Request.PathBase.Value;
            var model = new AbpApplicationPathViewComponentModel
            {
                ApplicationPath = applicationPath == null ? "/" : applicationPath.EnsureEndsWith('/')
            };

            return View("~/Pages/Shared/Components/AbpApplicationPath/Default.cshtml", model);
        }
    }
}
