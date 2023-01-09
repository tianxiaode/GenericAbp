using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Generic.Abp.PhoneLogin.MongoDB;

[DependsOn(
    typeof(GenericAbpPhoneLoginDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class GenericAbpPhoneLoginMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<GenericAbpPhoneLoginMongoDbModule>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
