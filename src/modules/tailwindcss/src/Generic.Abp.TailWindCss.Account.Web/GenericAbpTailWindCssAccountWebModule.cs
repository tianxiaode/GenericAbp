using Generic.Abp.TailWindCss.Account.Web.Bundling;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.TailWindCss.Account.Web;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpExceptionHandlingModule)
)]
public class GenericAbpTailWindCssAccountWebModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(AccountResource),
                typeof(GenericAbpTailWindCssAccountWebModule).Assembly);
        });


        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpTailWindCssAccountWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<TailWindTheme>();

            if (options.DefaultThemeName == null)
            {
                options.DefaultThemeName = TailWindTheme.Name;
            }
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpTailWindCssAccountWebModule>(
                "Generic.Abp.TailWindCss.Account.Web");
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(TailWindThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddContributors(typeof(TailWindGlobalStyleContributor));
                });

            options
                .ScriptBundles
                .Add(TailWindThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddContributors(typeof(TailWindGlobalScriptContributor));
                });
        });


        ConfigureProfileManagementPage();

        context.Services.AddAutoMapperObjectMapper<GenericAbpTailWindCssAccountWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<GenericAbpTailWindCssAccountWebAutoMapperProfile>(validate: true);
        });
    }

    private void ConfigureProfileManagementPage()
    {
        Configure<RazorPagesOptions>(options => { options.Conventions.AuthorizePage("/Account/Manage"); });
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() => { });
    }
}