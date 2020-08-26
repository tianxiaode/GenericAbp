using Generic.Abp.Demo.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.Demo
{
    [DependsOn(
        typeof(DemoEntityFrameworkCoreTestModule)
        )]
    public class DemoDomainTestModule : AbpModule
    {

    }
}