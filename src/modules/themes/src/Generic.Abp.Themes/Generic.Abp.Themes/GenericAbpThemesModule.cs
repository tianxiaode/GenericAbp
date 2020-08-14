using Generic.Abp.Themes.Bundling;
using Generic.Abp.Themes.Toolbars;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Themes
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiWidgetsModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule)
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
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpThemesModule>("Generic.Abp.Themes");
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(GenericThemeGlobalBundles.Styles.Global, bundle => { bundle.AddContributors(typeof(GenericAbpThemesModule)); });

                options
                    .ScriptBundles
                    .Add(GenericThemeGlobalBundles.Scripts.Global, bundle => bundle.AddContributors(typeof(GenericAbpThemesModule)));

                options
                    .StyleBundles
                    .Add(GenericThemeApplicationBundles.Styles.Application, bundle => { bundle.AddContributors(typeof(GenericAbpThemesModule)); });

                options
                    .ScriptBundles
                    .Add(GenericThemeApplicationBundles.Scripts.Application, bundle => bundle.AddContributors(typeof(GenericAbpThemesModule)));

            });

            Configure<AbpThemingOptions>(options =>
            {
                options.Themes.Add<GenericTheme>();

                if (options.DefaultThemeName == null)
                {
                    options.DefaultThemeName = GenericTheme.Name;
                }
            });


            Configure<AbpToolbarOptions>(options =>
            {
                options.Contributors.Add(new ExtThemeMainTopToolbarContributor());
            });


        }


    }
}
