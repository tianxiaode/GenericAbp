using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    public static class FileManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureFileManagement(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            var dbType = builder.GetDatabaseProvider();

            builder.Entity<FileInfoBase>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FileInfoBases",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.MimeType).IsRequired().HasMaxLength(FileConsts.MimeTypeMaxLength);
                b.Property(m => m.Extension).IsRequired().HasMaxLength(FileConsts.ExtensionMaxLength);
                b.Property(m => m.Hash).IsRequired().HasMaxLength(FileConsts.HashMaxLength);
                b.Property(m => m.Path).IsRequired().HasMaxLength(FileConsts.PathMaxLength);
                b.Property(m => m.Size).IsRequired().HasDefaultValue(0);


                //Indexes
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.Hash).IsUnique();
                b.HasIndex(m => m.Extension);
            });

            builder.Entity<Resource>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "Resources",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Name).IsRequired().HasMaxLength(ResourceConsts.NameMaxLength);
                b.Property(m => m.Code).IsRequired().HasMaxLength(ResourceConsts.CodeMaxLength);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.Name).UseCollation("gbk_chinese_ci");
                }

                //Relations
                //b.HasMany<File>().WithOne().HasForeignKey(m => m.Id).IsRequired();
                b.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId)
                    .IsRequired(false);

                //Indexes
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.Name);
                b.HasIndex(m => m.ParentId);

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<ResourcePermission>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "ResourcePermissions",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.ProviderName).IsRequired().HasMaxLength(ResourceConsts.ProviderNameMaxLength);
                b.Property(m => m.ProviderKey).HasMaxLength(ResourceConsts.ProviderKeyMaxLength);
                b.Property(m => m.Permissions).IsRequired().HasDefaultValue(0);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.ProviderName).UseCollation("ascii_general_ci");
                    b.Property(m => m.ProviderKey).UseCollation("ascii_general_ci");
                }

                //Indexes
                b.HasIndex(m => new { m.ResourceId, m.Permissions, m.ProviderName, m.ProviderKey });
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