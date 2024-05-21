using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.ExportManager.EntityFrameworkCore
{
    [ConnectionStringName(ExportManagerDbProperties.ConnectionStringName)]
    public class ExportManagerDbContext : AbpDbContext<ExportManagerDbContext>, IExportManagerDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public ExportManagerDbContext(DbContextOptions<ExportManagerDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureExportManager();
        }
    }
}