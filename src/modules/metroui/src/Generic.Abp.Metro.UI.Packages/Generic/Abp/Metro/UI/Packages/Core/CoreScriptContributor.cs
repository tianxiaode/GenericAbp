using System.Collections.Generic;
using Generic.Abp.Metro.UI.Packages.Utils;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Packages.Core;

[DependsOn(typeof(UtilsScriptContributor))]
public class CoreScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/abp/core/abp.js");
    }
}
