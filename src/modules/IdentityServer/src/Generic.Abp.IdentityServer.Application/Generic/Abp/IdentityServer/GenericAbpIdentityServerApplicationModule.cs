using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace Generic.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpIdentityServerDomainModule),
        typeof(GenericAbpIdentityServerApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class GenericAbpIdentityServerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpIdentityServerApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpIdentityServerApplicationModule>(validate: true);
            });
        }
    }
}
