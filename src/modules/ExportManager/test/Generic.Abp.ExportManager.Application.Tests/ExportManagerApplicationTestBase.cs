using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ExportManagerApplicationTestBase<TStartupModule> : ExportManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
