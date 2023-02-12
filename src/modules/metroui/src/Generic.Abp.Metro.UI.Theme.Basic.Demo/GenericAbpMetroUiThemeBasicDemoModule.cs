using Generic.Abp.Metro.UI.Theme.Basic.Demo.Localization;
using Generic.Abp.Metro.UI.Theme.Basic.Demo.Menu;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.Theme.Basic.Demo;

[DependsOn(
    typeof(AbpValidationModule),
    typeof(GenericAbpMetroUiThemeBasicModule))]
public class GenericAbpMetroUiThemeBasicDemoModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiThemeBasicDemoModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<MetroUiThemeBasicDemoResource>("en")
                //.AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Generic.Abp.Metro.UI.Theme.Basic.Demo",
                typeof(MetroUiThemeBasicDemoResource));
        });

        Configure<AbpNavigationOptions>(options => { options.MenuContributors.Add(new DemoMenuContributor()); });
    }
}