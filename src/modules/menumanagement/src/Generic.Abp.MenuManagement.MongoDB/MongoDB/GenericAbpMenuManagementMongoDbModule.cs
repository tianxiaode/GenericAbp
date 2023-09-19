using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Generic.Abp.MenuManagement.MongoDB;

[DependsOn(
    typeof(GenericAbpMenuManagementDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class GenericAbpMenuManagementMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<GenericAbpMenuManagementMongoDbModule>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
