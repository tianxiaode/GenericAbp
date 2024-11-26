using AspNet.Security.OAuth.GitHub;
using Generic.Abp.ExternalAuthentication.AuthenticationProviderHandlers;
using Generic.Abp.ExternalAuthentication.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using System.Text.Encodings.Web;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using static IdentityModel.ClaimComparer;

namespace Generic.Abp.ExternalAuthentication;

[DependsOn(
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpValidationModule),
    typeof(AbpCachingModule),
    typeof(AbpAutoMapperModule)
)]
public class GenericAbpExternalAuthenticationAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(ExternalAuthenticationResource),
                typeof(GenericAbpExternalAuthenticationAspNetCoreModule).Assembly);
        });

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