using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Generic.Abp.ExportManager
{
    [DependsOn(
        typeof(GenericAbpExportManagerDomainModule),
        typeof(GenericAbpExportManagerApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class GenericAbpExportManagerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpExportManagerApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpExportManagerApplicationModule>(validate: true);
            });
        }
    }
}
