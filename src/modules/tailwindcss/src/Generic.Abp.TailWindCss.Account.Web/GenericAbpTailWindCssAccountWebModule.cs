using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.Threading;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.TailWindCss.Account.Web;

[DependsOn(
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityAspNetCoreModule),
    typeof(AbpOpenIddictAspNetCoreModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpExceptionHandlingModule)
)]
public class GenericAbpTailWindCssAccountWebModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpTailWindCssAccountWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpTailWindCssAccountWebModule>();
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