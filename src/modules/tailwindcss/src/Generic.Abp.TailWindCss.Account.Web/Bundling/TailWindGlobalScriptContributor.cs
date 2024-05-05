using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.TailWindCss.Account.Web.Bundling;

public class TailWindGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/libs/lodash/lodash.min.js");
        context.Files.Add("/libs/cash/cash.min.js");
        context.Files.Add("/libs/abp/core/abp.js");
        context.Files.Add("/js/tailwind.js");
    }
}