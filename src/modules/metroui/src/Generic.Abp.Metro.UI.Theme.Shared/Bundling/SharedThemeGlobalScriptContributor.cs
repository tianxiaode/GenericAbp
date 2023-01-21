using Generic.Abp.Metro.UI.Packages.Lodash;
using Generic.Abp.Metro.UI.Packages.MetroUI;
using Generic.Abp.Metro.UI.Packages.SignalR;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Theme.Shared.Bundling;

[DependsOn(
    typeof(MetroUiScriptContributor),
    typeof(LodashScriptContributor),
    typeof(SignalRBrowserScriptContributor)
    )]
public class SharedThemeGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddRange(new []
        {
            "/libs/abp/metro/metro-extensions.js",
            "/libs/abp/metro/modal-manager.js"
        });
    }
}
