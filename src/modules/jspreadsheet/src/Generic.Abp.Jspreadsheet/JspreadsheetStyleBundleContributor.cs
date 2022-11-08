using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Jspreadsheet
{
    public class JspreadsheetStyleBundleContributor: BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/jspreadsheet/jexcel.css");
            //context.Files.AddIfNotContains("/libs/jspreadsheet/jexcel.theme.css");
            context.Files.AddIfNotContains("/libs/jsuites/jsuites.css");
        }

    }
}
