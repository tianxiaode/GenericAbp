using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Tailwind.Bundling;

public class TailwindGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/libs/lodash/lodash.min.js");
        context.Files.Add("/libs/abp/core/abp.js");
        context.Files.Add("/js/tailwind.js");
    }
}