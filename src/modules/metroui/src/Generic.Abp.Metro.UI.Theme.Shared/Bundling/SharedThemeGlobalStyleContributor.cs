using Generic.Abp.Metro.UI.Packages.Core;
using Generic.Abp.Metro.UI.Packages.MetroUI;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Theme.Shared.Bundling;

[DependsOn(
    typeof(MetroUiStyleContributor),
    typeof(CoreStyleContributor)
)]
public class SharedThemeGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        //context.Files.AddRange(new[]
        //{
        //    });
    }
}
