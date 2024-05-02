using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Generic.Abp.OAuthProviderManager.MongoDB;

[DependsOn(
    typeof(GenericAbpOAuthProviderManagerDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class GenericAbpOAuthProviderManagerMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<GenericAbpOAuthProviderManagerMongoDbModule>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
