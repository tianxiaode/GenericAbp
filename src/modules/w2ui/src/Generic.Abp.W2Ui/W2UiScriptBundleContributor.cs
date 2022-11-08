using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.W2Ui
{
    public class W2UiScriptBundleContributor: BundleContributor
    {
         public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/w2ui/w2ui-1.5.min.js");
            
        }

    }
}
