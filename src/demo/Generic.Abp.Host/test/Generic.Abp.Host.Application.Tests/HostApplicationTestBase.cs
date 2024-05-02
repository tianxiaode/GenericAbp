using Volo.Abp.Modularity;

namespace Generic.Abp.Host;

public abstract class HostApplicationTestBase<TStartupModule> : HostTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
