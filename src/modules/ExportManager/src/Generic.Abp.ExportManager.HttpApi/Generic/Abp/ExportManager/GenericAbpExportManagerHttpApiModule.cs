using Localization.Resources.AbpUi;
using Generic.Abp.ExportManager.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Generic.Abp.ExportManager
{
    [DependsOn(
        typeof(GenericAbpExportManagerApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GenericAbpExportManagerHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpExportManagerHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ExportManagerResource>();
            });
        }
    }
}
