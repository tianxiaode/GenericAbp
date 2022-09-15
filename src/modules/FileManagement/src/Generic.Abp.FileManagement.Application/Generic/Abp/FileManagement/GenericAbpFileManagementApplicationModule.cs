using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Generic.Abp.FileManagement
{
    [DependsOn(
        typeof(GenericAbpFileManagementDomainModule),
        typeof(GenericAbpFileManagementApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class GenericAbpFileManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpFileManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpFileManagementApplicationModule>(validate: true);
            });
        }
    }
}
