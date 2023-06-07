using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Tabulator;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
)]
public class GenericAbpTabulatorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GenericAbpTabulatorModule>("Generic.Abp.Tabulator");
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add("Tabulator", bundle => { bundle.AddContributors(typeof(TabulatorStyleBundleContributor)); });

            options
                .ScriptBundles
                .Add("Tabulator", bundle => bundle.AddContributors(typeof(TabulatorScriptBundleContributor)));
        });
    }
}