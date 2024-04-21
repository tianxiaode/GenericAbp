using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Generic.Abp.ExportManager.MongoDB;

[DependsOn(
    typeof(GenericAbpExportManagerDomainModule),
    typeof(AbpMongoDbModule)
    )]
public class GenericAbpExportManagerMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<GenericAbpExportManagerMongoDbModule>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
