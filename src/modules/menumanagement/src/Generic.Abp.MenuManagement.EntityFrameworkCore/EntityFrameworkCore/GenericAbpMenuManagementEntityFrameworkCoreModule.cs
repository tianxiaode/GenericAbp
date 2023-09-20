using Generic.Abp.MenuManagement.EntityFrameworkCore.Menus;
using Generic.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Modularity;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(GenericAbpMenuManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GenericAbpMenuManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MenuManagementDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
                options.AddRepository<Menu, MenuRepository>();
            });

            Configure<AbpEntityOptions>(options =>
            {
                options.Entity<Menu>(entityOptions =>
                {
                    entityOptions.DefaultWithDetailsFunc = query => query.Include(m => m.Parent);
                });
            });
        }
    }
}