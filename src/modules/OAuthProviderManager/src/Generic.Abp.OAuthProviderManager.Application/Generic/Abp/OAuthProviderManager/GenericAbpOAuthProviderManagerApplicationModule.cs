using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.OAuthProviderManager
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(GenericAbpOAuthProviderManagerApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class GenericAbpOAuthProviderManagerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpOAuthProviderManagerApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<GenericAbpOAuthProviderManagerApplicationModule>(validate: true);
            });
        }
    }
}