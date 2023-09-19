using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.MenuManagement.EntityFrameworkCore
{
    [ConnectionStringName(MenuManagementDbProperties.ConnectionStringName)]
    public class MenuManagementDbContext : AbpDbContext<MenuManagementDbContext>, IMenuManagementDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public MenuManagementDbContext(DbContextOptions<MenuManagementDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMenuManagement();
        }
    }
}