using Generic.Abp.Enumeration;
using Generic.Abp.Identity;
using Generic.Abp.MenuManagement;
using Generic.Abp.OpenIddict;
using QuickTemplate.Infrastructures;
using QuickTemplate.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace QuickTemplate;
//typeof(GenericAbpEnumerationDomainSharedModule),
//typeof(GenericAbpOpenIddictDomainSharedModule),
//typeof(GenericAbpPhoneLoginDomainSharedModule)

[DependsOn(
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpOpenIddictDomainSharedModule),
    typeof(AbpPermissionManagementDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(GenericAbpIdentityDomainSharedModule),
    typeof(GenericAbpEnumerationDomainSharedModule),
    typeof(GenericAbpOpenIddictDomainSharedModule),
    typeof(QuickTemplateInfrastructuresDomainSharedModule),
    typeof(GenericAbpMenuManagementDomainSharedModule)
)]
public class QuickTemplateDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        QuickTemplateGlobalFeatureConfigurator.Configure();
        QuickTemplateModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<QuickTemplateDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<QuickTemplateResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/QuickTemplate");

            options.DefaultResourceType = typeof(QuickTemplateResource);
        });


        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("QuickTemplate", typeof(QuickTemplateResource));
        });
    }
}