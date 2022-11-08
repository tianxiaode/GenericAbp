using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.W2Ui
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
    )]
    public class GenericAbpW2UiModule: AbpModule
    {


        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpW2UiModule>("Generic.Abp.W2Ui");

            });

            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add("W2Ui", bundle => { bundle.AddContributors(typeof(W2UiStyleBundleContributor)); });

                options
                    .ScriptBundles
                    .Add("W2Ui", bundle => bundle.AddContributors(typeof(W2UiScriptBundleContributor)));
  
                

            });
        }



    }
}