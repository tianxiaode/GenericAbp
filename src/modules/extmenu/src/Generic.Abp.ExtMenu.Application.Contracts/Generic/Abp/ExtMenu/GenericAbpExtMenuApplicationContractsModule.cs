using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

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

        }

    }
}
