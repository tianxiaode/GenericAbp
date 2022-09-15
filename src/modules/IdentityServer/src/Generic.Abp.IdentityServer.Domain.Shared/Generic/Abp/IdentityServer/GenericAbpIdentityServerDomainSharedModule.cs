using Generic.Abp.BusinessException;
using Generic.Abp.BusinessException.Localization;
using Generic.Abp.Enumeration;
using Generic.Abp.IdentityServer.Enumerations;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpIdentityServerDomainSharedModule),
        typeof(GenericAbpBusinessExceptionModule),
        typeof(GenericAbpEnumerationDomainSharedModule)
    )]
    public class GenericAbpIdentityServerDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpIdentityServerDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpIdentityServerResource>()
                    .AddBaseTypes(typeof(BusinessExceptionResource))
                    .AddVirtualJson("/Generic/Abp/IdentityServer/Localization/IdentityServer");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.IdentityServer", typeof(AbpIdentityServerResource));
            });

            Configure<EnumerationOptions>(options =>
            {
                options.Resources.Add(typeof(SecretType));
            });
        }
    }
}
