using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Themes.Bundling
{
    public class GenericThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/themes/generic/layout.js");
        }
    }
}
