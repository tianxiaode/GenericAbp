using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Jspreadsheet
{
    public class JspreadsheetScriptBundleContributor : BundleContributor
    {
         public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/jspreadsheet/jexcel.js");
        }

    }
}
