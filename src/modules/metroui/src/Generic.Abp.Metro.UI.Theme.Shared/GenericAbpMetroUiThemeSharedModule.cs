using Generic.Abp.Metro.UI.Packages;
using Generic.Abp.Metro.UI.Theme.Shared.Bundling;
using Generic.Abp.Metro.UI.Theme.Shared.ProxyScripting.Generators;
using Generic.Abp.Metro.UI.Widgets;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Metro.UI.Theme.Shared;

[DependsOn(
    typeof(GenericAbpMetroUiModule),
    typeof(Generic.Abp.Metro.UI.Packages.GenericAbpMetroUiPackagesModule),
    typeof(GenericAbpMetroUiWidgetsModule)
)]
public class GenericAbpMetroUiThemeSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(GenericAbpMetroUiThemeSharedModule).Assembly);
        });
    }


    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpApiProxyScriptingOptions>(options =>
        {
            options.Generators[MetroProxyScriptGenerator.Name] = typeof(MetroProxyScriptGenerator);
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpMetroUiThemeSharedModule>("Generic.Abp.Metro.UI.Theme.Shared");
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(StandardBundles.Styles.Global,
                    bundle => { bundle.AddContributors(typeof(SharedThemeGlobalStyleContributor)); }
                );

            options
                .ScriptBundles
                .Add(StandardBundles.Scripts.Global,
                    bundle => bundle.AddContributors(typeof(SharedThemeGlobalScriptContributor)));
        });
    }
}