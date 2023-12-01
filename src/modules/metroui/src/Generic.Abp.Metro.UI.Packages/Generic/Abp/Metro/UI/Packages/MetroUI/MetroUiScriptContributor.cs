using System.Collections.Generic;
using Generic.Abp.Metro.UI.Packages.Core;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Packages.MetroUI
{
    [DependsOn(typeof(CoreScriptContributor))]
    public class MetroUiScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/metro/metro.min.js");
            context.Files.AddIfNotContains("/libs/metro/abp.metro.js");
        }
    }
}