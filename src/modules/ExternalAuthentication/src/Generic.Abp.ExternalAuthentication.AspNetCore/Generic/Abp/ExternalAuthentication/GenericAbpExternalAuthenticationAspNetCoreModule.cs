using Generic.Abp.ExternalAuthentication.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.ExternalAuthentication;

[DependsOn(
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpValidationModule),
    typeof(AbpAutoMapperModule)
)]
public class GenericAbpExternalAuthenticationAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpExternalAuthenticationAspNetCoreModule)
                .Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpExternalAuthenticationAspNetCoreModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<ExternalAuthenticationResource>("en")
                .AddBaseTypes(typeof(IdentityResource))
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Generic/Abp/ExternalAuthentication/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Generic.Abp.ExternalAuthentication", typeof(ExternalAuthenticationResource));
        });
    }
}