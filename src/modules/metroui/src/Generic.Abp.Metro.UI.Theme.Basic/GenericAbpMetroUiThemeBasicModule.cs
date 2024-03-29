﻿using Generic.Abp.Metro.UI.MultiTenancy;
using Generic.Abp.Metro.UI.Theme.Basic.Bundling;
using Generic.Abp.Metro.UI.Theme.Basic.Toolbars;
using Generic.Abp.Metro.UI.Theme.Shared.Bundling;
using Generic.Abp.Metro.UI.Theme.Shared.Toolbars;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.Theme.Basic;

[DependsOn(
    typeof(GenericAbpMetroUiMultiTenancyModule)
)]
public class GenericAbpMetroUiThemeBasicModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiThemeBasicModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<BasicTheme>();

            options.DefaultThemeName ??= BasicTheme.Name;
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiThemeBasicModule>("Generic.Abp.Metro.UI.Theme.Basic");
        });

        Configure<MetroToolbarOptions>(options =>
        {
            options.Contributors.Add(new BasicThemeMainTopToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BasicThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(StandardBundles.Styles.Global)
                        .AddContributors(typeof(BasicThemeGlobalStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BasicThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        .AddBaseBundles(StandardBundles.Scripts.Global)
                        .AddContributors(typeof(BasicThemeGlobalScriptContributor));
                });
        });
    }
}