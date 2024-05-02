using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.TailWindCss.Account.Web.Bundling;

public class TailWindGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/Themes/TailWind/Global/styles/main.css");
    }
}