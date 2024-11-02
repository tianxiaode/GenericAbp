using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.OpenIddict;

namespace Generic.Abp.OpenIddict
{
    [DependsOn(
        typeof(GenericAbpOpenIddictApplicationContractsModule),
        typeof(AbpOpenIddictDomainModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class GenericAbpOpenIddictApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpOpenIddictApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpOpenIddictApplicationModule>(validate: true);
            });
        }
    }
}