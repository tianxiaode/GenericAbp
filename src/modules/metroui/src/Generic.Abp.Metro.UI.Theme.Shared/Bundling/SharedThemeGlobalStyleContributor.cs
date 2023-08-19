using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Theme.Shared.Bundling;

[DependsOn(
    typeof(Packages.MetroUI.MetroUiStyleContributor),
    typeof(Packages.Core.CoreStyleContributor)
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