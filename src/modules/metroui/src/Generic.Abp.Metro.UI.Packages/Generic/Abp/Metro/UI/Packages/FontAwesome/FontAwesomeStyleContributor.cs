using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Packages.FontAwesome;

public class FontAwesomeStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/fontawesome/css/all.min.css");
        context.Files.AddIfNotContains("/libs/fontawesome/css/v4-shims.min.css");
    }
}