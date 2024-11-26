using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QuickTemplate.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class QuickTemplateDbContextFactory : IDesignTimeDbContextFactory<QuickTemplateDbContext>
{
    public QuickTemplateDbContext CreateDbContext(string[] args)
    {
        QuickTemplateEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        // var builder =
        //     new DbContextOptionsBuilder<QuickTemplateDbContext>(new DbContextOptions<QuickTemplateDbContext>())
        //         .UseMySql(configuration.GetConnectionString("Default"),
        //             ServerVersion.Create(new Version(10, 5, 8),
        //                 Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MariaDb),
        //             options => options.EnableStringComparisonTranslations());
        var builder =
            new DbContextOptionsBuilder<QuickTemplateDbContext>(new DbContextOptions<QuickTemplateDbContext>())
                .UseSqlServer(configuration.GetConnectionString("Default"));

        return new QuickTemplateDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../QuickTemplate.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}