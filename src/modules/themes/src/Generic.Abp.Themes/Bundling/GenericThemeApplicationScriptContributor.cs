using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Themes.Bundling
{
    public class GenericThemeApplicationScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/datatables/datatables.min.js",
            });
        }

    }
}
