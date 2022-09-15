using System;
using Generic.Abp.FileManagement.Files;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    public static class FileManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureFileManagement(
            this ModelBuilder builder,
            Action<FileManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FileManagementModelBuilderConfigurationOptions(
                FileManagementDbProperties.DbTablePrefix,
                FileManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Files.File>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Files", options.Schema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Filename).IsRequired().HasMaxLength(FileConsts.FilenameMaxLength)
                    .UseCollation("gbk_chinese_ci");
                b.Property(m => m.MimeType).IsRequired().HasMaxLength(FileConsts.MimeTypeMaxLength)
                    .UseCollation("ascii_general_ci");
                b.Property(m => m.FileType).IsRequired().HasMaxLength(FileConsts.FileTypeMaxLength)
                    .UseCollation("ascii_general_ci");
                b.Property(m => m.Hash).IsRequired().HasMaxLength(FileConsts.HashMaxLength)
                    .UseCollation("ascii_general_ci");
                b.Property(m => m.Path).IsRequired().HasMaxLength(FileConsts.PathMaxLength)
                    .UseCollation("ascii_general_ci");
                b.Property(m => m.Description).IsRequired().HasMaxLength(FileConsts.DescriptionMaxLength);
                b.Property(m => m.Size).IsRequired().HasDefaultValue(0);


                //Indexes
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.Hash).IsUnique();

            });

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
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