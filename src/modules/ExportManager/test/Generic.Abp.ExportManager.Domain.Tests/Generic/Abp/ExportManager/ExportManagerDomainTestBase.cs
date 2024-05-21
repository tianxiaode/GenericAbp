using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ExportManagerDomainTestBase<TStartupModule> : ExportManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
