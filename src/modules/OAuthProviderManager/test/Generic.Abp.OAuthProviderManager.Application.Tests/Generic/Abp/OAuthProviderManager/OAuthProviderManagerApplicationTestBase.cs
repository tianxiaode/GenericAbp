using Volo.Abp.Modularity;

namespace Generic.Abp.OAuthProviderManager;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class OAuthProviderManagerApplicationTestBase<TStartupModule> : OAuthProviderManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
