using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace Generic.Abp.Metro.UI.Bundling;

[DependsOn(
    typeof(GenericAbpMetroUiModule),
    typeof(AbpMinifyModule),
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
    )]
public class GenericAbpMetroUiBundlingModule : AbpModule
{

}
