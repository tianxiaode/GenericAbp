using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Generic.Abp.OAuthProviderManager.MongoDB;

public static class OAuthProviderManagerMongoDbContextExtensions
{
    public static void ConfigureOAuthProviderManager(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
