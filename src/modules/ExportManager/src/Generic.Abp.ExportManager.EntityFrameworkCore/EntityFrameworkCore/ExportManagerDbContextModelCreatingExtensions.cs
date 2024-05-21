using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Generic.Abp.ExportManager.EntityFrameworkCore
{
    public static class ExportManagerDbContextModelCreatingExtensions
    {
        public static void ConfigureExportManager(
            this ModelBuilder builder,
            Action<ExportManagerModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new ExportManagerModelBuilderConfigurationOptions(
                ExportManagerDbProperties.DbTablePrefix,
                ExportManagerDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.ExportSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */
        }
    }
}