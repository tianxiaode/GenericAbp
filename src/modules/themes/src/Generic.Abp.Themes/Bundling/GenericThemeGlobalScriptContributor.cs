using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Themes.Bundling
{
    [DependsOn(
        )]
    public class GenericThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/abp/utils/abp-utils.umd.min.js",
                "/libs/abp/core/abp.js",
                "/libs/jquery/jquery-3.5.1.min",
                "/libs/abp/jquery/abp.jquery.js",
                "/libs/bootstrap5/js/bootstrap.bundle.min.js",
                "/libs/jquery-validation/jquery.validate.js",
                "/libs/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
                "/libs/jquery-form/jquery.form.min.js",
            });
        }
    }
}
