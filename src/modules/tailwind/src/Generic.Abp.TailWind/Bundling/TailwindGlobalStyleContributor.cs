using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Tailwind.Bundling;

public class TailwindGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/libs/fontawesome/css/all.min.css");
        context.Files.Add("/libs/fontawesome/css/v4-shims.min.css");
        context.Files.Add("/libs/abp/core/abp.css");
        context.Files.Add("/css/tailwind.css");
    }
}