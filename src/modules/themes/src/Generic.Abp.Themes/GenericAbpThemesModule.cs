using Generic.Abp.Themes.Bundling;
using Generic.Abp.Themes.Shared;
using Generic.Abp.Themes.Shared.Bundling;
using Generic.Abp.Themes.Shared.Toolbars;
using Generic.Abp.Themes.Toolbars;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Themes
{
    [DependsOn(
        //typeof(AbpAspNetCoreMvcUiWidgetsModule),
        //typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(GenericAbpThemeSharedModule)
    )]

    public class GenericAbpThemesModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpThemesModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<GenericTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = GenericTheme.Name;
                }
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpThemesModule>("Generic.Abp.Themes");
            });


            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new GenericThemeMainTopToolbarContributor());
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(GenericThemeBundles.Styles.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Styles.Global)
                            .AddContributors(typeof(GenericThemeGlobalStyleContributor));
                    });

                options
                    .ScriptBundles
                    .Add(GenericThemeBundles.Scripts.Global, bundle =>
                    {
                        bundle
                            .AddBaseBundles(StandardBundles.Scripts.Global)
                            .AddContributors(typeof(GenericThemeGlobalScriptContributor));
                    });
            });

        }


    }
}
