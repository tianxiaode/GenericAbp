using Generic.Abp.Themes.Shared.Packages.Bootstrap;
using Generic.Abp.Themes.Shared.Packages.BootstrapDatepicker;
using Generic.Abp.Themes.Shared.Packages.DatatablesNet;
using Generic.Abp.Themes.Shared.Packages.JQuery;
using Generic.Abp.Themes.Shared.Packages.JQueryForm;
using Generic.Abp.Themes.Shared.Packages.JQueryValidationUnobtrusive;
using Generic.Abp.Themes.Shared.Packages.Lodash;
using Generic.Abp.Themes.Shared.Packages.Luxon;
using Generic.Abp.Themes.Shared.Packages.MalihuCustomScrollbar;
using Generic.Abp.Themes.Shared.Packages.SweetAlert;
using Generic.Abp.Themes.Shared.Packages.Timeago;
using Generic.Abp.Themes.Shared.Packages.Toastr;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Generic.Abp.Themes.Shared.Bundling
{
    [DependsOn(
        typeof(JQueryScriptContributor),
        typeof(BootstrapScriptContributor),
        typeof(LodashScriptContributor),
        typeof(JQueryValidationUnobtrusiveScriptContributor),
        typeof(JQueryFormScriptContributor),
        //typeof(Select2ScriptContributor),
        typeof(DatatablesNetScriptContributor),
        typeof(SweetalertScriptContributor),
        typeof(ToastrScriptBundleContributor),
        typeof(MalihuCustomScrollbarPluginScriptBundleContributor),
        typeof(LuxonScriptContributor),
        typeof(TimeagoScriptContributor),
        typeof(BootstrapDatepickerScriptContributor)
        )]
    public class SharedThemeGlobalScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddRange(new[]
            {
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/ui-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/jquery/jquery-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/jquery-form/jquery-form-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/jquery/widget-manager.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/bootstrap/dom-event-handlers.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/bootstrap/modal-manager.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/datatables/datatables-extensions.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/sweetalert/abp-sweetalert.js",
                "/libs/abp/aspnetcore-mvc-ui-theme-shared/toastr/abp-toastr.js"
            });
        }
    }
}
