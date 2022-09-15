using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpFileManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpFileManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FileManagementDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Files.File, EfCoreFileRepository>();
            });
        }
    }
}