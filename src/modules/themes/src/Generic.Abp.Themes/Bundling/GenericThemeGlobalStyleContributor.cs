using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Themes.Bundling
{
    public class GenericThemeGlobalStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/fontawesome/css/all.min.css",
                "/libs/bootstrap5/css/bootstrap.min.css"
            });
        }
    }
}
