﻿using Generic.Abp.Demo.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Generic.Abp.Demo.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class DemoPageModel : AbpPageModel
    {
        protected DemoPageModel()
        {
            LocalizationResourceType = typeof(DemoResource);
        }
    }
}