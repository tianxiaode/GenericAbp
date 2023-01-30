using Localization.Resources.AbpUi;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiModule))]
    public class GenericAbpMetroUiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpMetroUiModule>("Generic.Abp.Metro.UI");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpUiResource>()
                    .AddVirtualJson("/Localization");
            });
        }
    }
}