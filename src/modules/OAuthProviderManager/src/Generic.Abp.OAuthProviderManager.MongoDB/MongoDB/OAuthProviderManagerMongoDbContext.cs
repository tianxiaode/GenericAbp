using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Generic.Abp.OAuthProviderManager.MongoDB;

[ConnectionStringName(OAuthProviderManagerDbProperties.ConnectionStringName)]
public class OAuthProviderManagerMongoDbContext : AbpMongoDbContext, IOAuthProviderManagerMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureOAuthProviderManager();
    }
}
