using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpIdentityApplicationModule),
        typeof(GenericAbpIdentityApplicationContractsModule),
        typeof(GenericAbpIdentityDomainModule),
        typeof(AbpAutoMapperModule)
    )]
    public class GenericAbpIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<GenericAbpIdentityApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IdentityApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}