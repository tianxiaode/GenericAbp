using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Generic.Abp.MyProjectName.MongoDB;

[DependsOn(
    typeof(GenericAbpMyProjectNameDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class GenericAbpMyProjectNameMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<GenericAbpMyProjectNameMongoDbModule>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
