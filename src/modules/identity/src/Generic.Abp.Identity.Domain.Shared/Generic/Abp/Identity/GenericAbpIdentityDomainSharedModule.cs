using Generic.Abp.BusinessException;
using Generic.Abp.Domain;
using Generic.Abp.Enumeration;
using Generic.Abp.Identity.Enumerations;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Identity
{
    [DependsOn(
        typeof(GenericAbpBusinessExceptionModule),
        typeof(GenericAbpDddDomainSharedModule),
        typeof(GenericAbpEnumerationDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpIdentityDomainSharedModule)
    )]
    public class GenericAbpIdentityDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpIdentityDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<IdentityResource>()
                    .AddVirtualJson("/Generic/Abp/Identity/Localization/Identity");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Generic.Abp.Identity", typeof(IdentityResource));
            });

            Configure<EnumerationOptions>(options =>
            {
                options
                    .Resources
                    .Add(typeof(SelectedOrNot));

            });

        }
    }
}
