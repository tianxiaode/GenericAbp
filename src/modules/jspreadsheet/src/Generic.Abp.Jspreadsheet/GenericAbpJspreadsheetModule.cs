using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.Jspreadsheet
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcUiBundlingAbstractionsModule)
    )]

    public class GenericAbpJspreadsheetModule : AbpModule
    {
         public override void ConfigureServices(ServiceConfigurationContext context)
        {

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpJspreadsheetModule>("Generic.Abp.Jspreadsheet");
            });


            Configure<AbpBundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add("Jspreadsheet", bundle => { bundle.AddContributors(typeof(JspreadsheetStyleBundleContributor)); });

                options
                    .ScriptBundles
                    .Add("Jspreadsheet", bundle => bundle.AddContributors(typeof(JspreadsheetScriptBundleContributor)));
  
                //options.ScriptBundles.Configure("Jspreadsheet", bundle => {
                //    bundle.AddContributors(typeof(JspreadsheetScriptBundleContributor));
                //});

                //options.StyleBundles.Configure("Jspreadsheet", bundle =>
                //{
                //    bundle.AddContributors(typeof(JspreadsheetStyleBundleContributor));
                //});
                

            });
        }



    }
}
