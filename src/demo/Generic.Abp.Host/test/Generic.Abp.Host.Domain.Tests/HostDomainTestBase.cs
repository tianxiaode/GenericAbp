using Volo.Abp.Modularity;

namespace Generic.Abp.Host;

/* Inherit from this class for your domain layer tests. */
public abstract class HostDomainTestBase<TStartupModule> : HostTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
