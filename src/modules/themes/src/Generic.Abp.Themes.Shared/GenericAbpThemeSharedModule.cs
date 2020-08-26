using Generic.Abp.Themes.Shared.Bundling;
using Generic.Abp.Themes.Shared.Packages.BootstrapDatepicker;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Themes.Shared
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpAspNetCoreMvcUiBundlingModule),
        typeof(AbpAspNetCoreMvcUiWidgetsModule)
        )]
    public class GenericAbpThemeSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpThemeSharedModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpThemeSharedModule>("Generic.Abp.Themes.Shared");
            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(StandardBundles.Styles.Global, bundle => { bundle.AddContributors(typeof(SharedThemeGlobalStyleContributor)); });

                options
                    .ScriptBundles
                    .Add(StandardBundles.Scripts.Global, bundle => bundle.AddContributors(typeof(SharedThemeGlobalScriptContributor)));
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.AddLanguagesMapOrUpdate(BootstrapDatepickerScriptContributor.PackageName,
                    new NameValue("zh-Hans", "zh-CN"),
                    new NameValue("zh-Hant", "zh-TW"));

                options.AddLanguageFilesMapOrUpdate(BootstrapDatepickerScriptContributor.PackageName,
                    new NameValue("zh-Hans", "zh-CN"),
                    new NameValue("zh-Hant", "zh-TW"));
            });

        }
    }
}
