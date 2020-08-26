using Volo.Abp.Modularity;

namespace Generic.Abp.Demo
{
    [DependsOn(
        typeof(DemoApplicationModule),
        typeof(DemoDomainTestModule)
        )]
    public class DemoApplicationTestModule : AbpModule
    {

    }
}