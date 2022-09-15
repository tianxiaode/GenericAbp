using Volo.Abp.Guids;
using Volo.Abp.Modularity;

namespace Generic.Abp.Helper
{
    [DependsOn(typeof(AbpGuidsModule))]
    public class GenericAbpHelperFileModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {


        }

    }
}
