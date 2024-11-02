using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.Localization;

namespace Generic.Abp.MenuManagement
{
    [DependsOn(
        typeof(GenericAbpMenuManagementDomainModule),
        typeof(GenericAbpMenuManagementApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpLocalizationModule)
    )]
    public class GenericAbpMenuManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpMenuManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpMenuManagementApplicationModule>(validate: true);
            });
        }
    }
}