using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule), typeof(AbpMinifyModule),typeof(AbpAspNetCoreMvcUiModule))]
    public class GenericAbpMetroUiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpMetroUiModule>("Generic.Abp.Metro.UI");
            });
        }

    }
}