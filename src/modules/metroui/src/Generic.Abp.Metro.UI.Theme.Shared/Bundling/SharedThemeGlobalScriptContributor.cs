using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Theme.Shared.Bundling;

[DependsOn(
    typeof(Generic.Abp.Metro.UI.Packages.MetroUI.MetroUiScriptContributor),
    typeof(Generic.Abp.Metro.UI.Packages.Lodash.LodashScriptContributor),
    typeof(Generic.Abp.Metro.UI.Packages.SignalR.SignalRBrowserScriptContributor)
)]
public class SharedThemeGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddRange(new BundleFile[]
        {
            "/libs/abp/metro/metro-alert.js",
            "/libs/abp/metro/metro-crud-toolbar.js",
            "/libs/abp/metro/metro-modal.js",
            "/libs/abp/metro/metro-extensions.js",
            "/libs/abp/metro/modal-manager.js"
        });
    }
}