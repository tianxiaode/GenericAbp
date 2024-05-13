using Generic.Abp.Tailwind.Bundling;
using Generic.Abp.Tailwind.Localization;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Threading;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Tailwind;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpExceptionHandlingModule)
)]
public class GenericAbpTailwindModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(TailwindResource),
                typeof(GenericAbpTailwindModule).Assembly);
        });


        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpTailwindModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<TailwindTheme>();

            options.DefaultThemeName ??= TailwindTheme.Name;
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpTailwindModule>(
                "Generic.Abp.Tailwind");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TailwindResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddBaseTypes(typeof(AccountResource))
                .AddVirtualJson("/Localization/Resources");
        });


        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Generic.Abp.Tailwind", typeof(TailwindResource));
        });


        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(TailwindThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddContributors(typeof(TailwindGlobalStyleContributor));
                });

            options
                .ScriptBundles
                .Add(TailwindThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddContributors(typeof(TailwindGlobalScriptContributor));
                });
        });


        context.Services.AddAutoMapperObjectMapper<GenericAbpTailwindModule>();
        Configure<AbpAutoMapperOptions>(options => { options.AddProfile<TailwindAutoMapperProfile>(validate: true); });
    }


    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() => { });
    }
}