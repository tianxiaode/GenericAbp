using Generic.Abp.Themes.Shared.Packages.Bootstrap;
using Generic.Abp.Themes.Shared.Packages.BootstrapDatepicker;
using Generic.Abp.Themes.Shared.Packages.Core;
using Generic.Abp.Themes.Shared.Packages.FontAwesome;
using Generic.Abp.Themes.Shared.Packages.MalihuCustomScrollbar;
using Generic.Abp.Themes.Shared.Packages.Toastr;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Themes.Shared.Bundling
{
    [DependsOn(
        typeof(CoreStyleContributor),
        typeof(BootstrapStyleContributor),
        typeof(FontAwesomeStyleContributor),
        typeof(ToastrStyleBundleContributor),
        //typeof(Select2StyleContributor),
        typeof(MalihuCustomScrollbarPluginStyleBundleContributor),
        //typeof(DatatablesNetBs4StyleContributor),
        typeof(BootstrapDatepickerStyleContributor)
    )]
    public class SharedThemeGlobalStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/datatables/css/datatables.css",
                "/libs/datatables/select/css/select.dataTables.css"
            });
        }
    }
}
