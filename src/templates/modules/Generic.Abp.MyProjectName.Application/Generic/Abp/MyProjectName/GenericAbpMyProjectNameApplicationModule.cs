using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace Generic.Abp.MyProjectName
{
    [DependsOn(
        typeof(GenericAbpMyProjectNameDomainModule),
        typeof(GenericAbpMyProjectNameApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class GenericAbpMyProjectNameApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpMyProjectNameApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpMyProjectNameApplicationModule>(validate: true);
            });
        }
    }
}
