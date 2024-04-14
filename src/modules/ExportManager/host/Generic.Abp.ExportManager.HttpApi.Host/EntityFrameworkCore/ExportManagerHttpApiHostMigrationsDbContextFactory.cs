using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Generic.Abp.ExportManager.EntityFrameworkCore;

public class ExportManagerHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<ExportManagerHttpApiHostMigrationsDbContext>
{
    public ExportManagerHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ExportManagerHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("ExportManager"));

        return new ExportManagerHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
