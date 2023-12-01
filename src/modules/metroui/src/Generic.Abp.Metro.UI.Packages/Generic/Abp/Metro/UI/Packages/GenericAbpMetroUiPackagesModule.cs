using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Packages
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule),
        typeof(AbpLocalizationModule)
    )]
    public class GenericAbpMetroUiPackagesModule : AbpModule
    {
    }
}