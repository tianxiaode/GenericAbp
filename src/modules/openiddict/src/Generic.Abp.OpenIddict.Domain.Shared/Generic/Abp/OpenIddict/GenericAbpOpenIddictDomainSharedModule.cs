using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Generic.Abp.OpenIddict.Localization;
using Generic.Abp.BusinessException;
using Generic.Abp.BusinessException.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.OpenIddict;

namespace Generic.Abp.OpenIddict
{
    [DependsOn(
        typeof(GenericAbpBusinessExceptionModule),
        typeof(AbpOpenIddictDomainSharedModule)
    )]
    public class GenericAbpOpenIddictDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpOpenIddictDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<OpenIddictResource>("en")
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddVirtualJson("/Generic/Abp/OpenIddict/Localization/OpenIddict");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.OpenIddict", typeof(OpenIddictResource));
            });
        }
    }
}
