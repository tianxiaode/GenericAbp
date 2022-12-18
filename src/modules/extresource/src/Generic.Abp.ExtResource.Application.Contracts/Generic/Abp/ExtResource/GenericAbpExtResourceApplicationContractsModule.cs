using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.ExtResource
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule)
    )]

    public class GenericAbpExtResourceApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        	Configure<MenuOptions>(context.Services.GetConfiguration().GetSection("Menus"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpExtResourceApplicationContractsModule>("Generic.Abp.ExtResource");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<Localization.ExtResource>("en")
                    .AddVirtualJson("/Localization/ExtResource");
            });

            

        }

    }
}