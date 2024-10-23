using Generic.Abp.OpenIddict.Localization;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.OpenIddict
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpOpenIddictDomainSharedModule),
        typeof(AbpAuthorizationModule)
    )]
    public class GenericAbpOpenIddictApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpOpenIddictApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<OpenIddictResource>("en")
                    .AddVirtualJson("/Generic/Abp/OpenIddict/Localization/Resources");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.OpenIddict", typeof(OpenIddictResource));
            });
        }
    }
}