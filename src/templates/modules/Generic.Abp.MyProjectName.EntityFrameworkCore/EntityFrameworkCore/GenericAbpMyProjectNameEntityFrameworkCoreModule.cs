using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.MyProjectName.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpMyProjectNameDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpMyProjectNameEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyProjectNameDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}