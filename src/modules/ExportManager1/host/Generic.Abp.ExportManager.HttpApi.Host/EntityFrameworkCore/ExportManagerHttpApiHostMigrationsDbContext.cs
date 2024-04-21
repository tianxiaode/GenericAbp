using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.ExportManager.EntityFrameworkCore;

public class ExportManagerHttpApiHostMigrationsDbContext : AbpDbContext<ExportManagerHttpApiHostMigrationsDbContext>
{
    public ExportManagerHttpApiHostMigrationsDbContext(DbContextOptions<ExportManagerHttpApiHostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureExportManager();
    }
}
