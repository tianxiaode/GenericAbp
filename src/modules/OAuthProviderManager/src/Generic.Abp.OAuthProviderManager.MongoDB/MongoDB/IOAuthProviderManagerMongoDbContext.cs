using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Generic.Abp.OAuthProviderManager.MongoDB;

[ConnectionStringName(OAuthProviderManagerDbProperties.ConnectionStringName)]
public interface IOAuthProviderManagerMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
