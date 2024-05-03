using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.TailWindCss.Account.Web.Bundling;

public class TailWindGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/Pages/scripts/main.js");
    }
}