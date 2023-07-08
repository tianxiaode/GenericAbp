using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.W2Ui
{
    public class W2UiStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/w2ui/w2ui.min.css");
            context.Files.AddIfNotContains("/libs/w2ui/fix.css");
        }
    }
}