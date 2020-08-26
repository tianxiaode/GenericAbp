using Generic.Abp.Account.Localization;
using Generic.Abp.Themes.Shared;
using Generic.Abp.Themes.Shared.Toolbars;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Account.Web
{
    [DependsOn(
        typeof(GenericAccountHttpApiModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpAutoMapperModule),
        typeof(GenericAbpThemeSharedModule)
        )]
    public class GenericAccountWebModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(typeof(AccountResource), typeof(GenericAccountWebModule).Assembly);
            });

            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAccountWebModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAccountWebModule>("Generic.Abp.Account.Web");
            });

            Configure<AbpNavigationOptions>(options =>
            {
                options.MenuContributors.Add(new AbpAccountUserMenuContributor());
            });

            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new AccountModuleToolbarContributor());
            });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Account/Manage");
            });

            context.Services.AddAutoMapperObjectMapper<GenericAccountWebModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpAccountWebAutoMapperProfile>(validate: true);
            });
        }
    }
}
