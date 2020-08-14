using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Themes.Bundling
{
    public class GenericThemeApplicationStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/datatables/datatables.min.css",
            });
        }

    }
}
