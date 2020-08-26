using Generic.Abp.ExtMenu.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Generic.Abp.ExtMenu
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule)
    )]

    public class GenericAbpExtMenuApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<MenuOptions>(context.Services.GetConfiguration().GetSection("Menus"));

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GenericAbpExtMenuApplicationContractsModule>("Generic.Abp.ExtMenu");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ExtMenuResource>("en")
                    .AddVirtualJson("/Localization/ExtMenu");
            });

        }

    }
}
