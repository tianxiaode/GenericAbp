using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.VirtualPaths;
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
                b.Property(m => m.FileType).IsRequired().HasMaxLength(FileConsts.FileTypeMaxLength);
                b.Property(m => m.Hash).IsRequired().HasMaxLength(FileConsts.HashMaxLength);
                b.Property(m => m.Path).IsRequired().HasMaxLength(FileConsts.PathMaxLength);
                b.Property(m => m.Size).IsRequired().HasDefaultValue(0);


                //Indexes
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.Hash).IsUnique();
                b.HasIndex(m => m.FileType);
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<File>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "Files", FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Filename).IsRequired().HasMaxLength(FileConsts.FilenameMaxLength);
                b.Property(m => m.Hash).IsRequired().HasMaxLength(FileConsts.HashMaxLength);
                b.Property(m => m.Description).IsRequired().HasMaxLength(FileConsts.DescriptionMaxLength);
                b.Property(m => m.IsInheritPermissions).IsRequired().HasDefaultValue(false);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.Filename).UseCollation("gbk_chinese_ci");
                }

                //Indexes
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => new { m.FolderId, m.FileInfoBase });
                b.HasIndex(m => new { m.FolderId, m.Filename });
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<FilePermission>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FilePermissions",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.ProviderName).IsRequired().HasMaxLength(FileConsts.ProviderNameMaxLength);
                b.Property(m => m.ProviderKey).HasMaxLength(FileConsts.ProviderKeyMaxLength);
                b.Property(m => m.CanRead).IsRequired().HasDefaultValue(false);
                b.Property(m => m.CanWrite).IsRequired().HasDefaultValue(false);
                b.Property(m => m.CanDelete).IsRequired().HasDefaultValue(false);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.ProviderName).UseCollation("ascii_general_ci");
                    b.Property(m => m.ProviderKey).UseCollation("ascii_general_ci");
                }

                //Indexes
                b.HasIndex(m => new { m.TargetId, m.ProviderName, m.ProviderKey });
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<Folder>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "Folders",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Name).IsRequired().HasMaxLength(FolderConsts.NameMaxLength);
                b.Property(m => m.Code).IsRequired().HasMaxLength(FolderConsts.CodeMaxLength);
                b.Property(m => m.AllowedFileTypes).IsRequired().HasMaxLength(FolderConsts.AllowedFileTypesMaxLength);
                b.Property(m => m.StorageQuota).IsRequired().HasDefaultValue(0);
                b.Property(m => m.UsedStorage).IsRequired().HasDefaultValue(0);
                b.Property(m => m.MaxFileSize).IsRequired().HasDefaultValue(0);
                b.Property(m => m.IsInheritPermissions).IsRequired().HasDefaultValue(false);

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

            builder.Entity<FolderPermission>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "FolderPermissions",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.ProviderName).IsRequired().HasMaxLength(FolderConsts.ProviderNameMaxLength);
                b.Property(m => m.ProviderKey).HasMaxLength(FolderConsts.ProviderKeyMaxLength);
                b.Property(m => m.CanRead).IsRequired().HasDefaultValue(false);
                b.Property(m => m.CanWrite).IsRequired().HasDefaultValue(false);
                b.Property(m => m.CanDelete).IsRequired().HasDefaultValue(false);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.ProviderName).UseCollation("ascii_general_ci");
                    b.Property(m => m.ProviderKey).UseCollation("ascii_general_ci");
                }

                //Indexes
                b.HasIndex(m => new { m.TargetId, m.ProviderName, m.ProviderKey });
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<VirtualPath>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "VirtualPaths",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Path).IsRequired().HasMaxLength(VirtualPathConsts.PathMaxLength);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.Path).UseCollation("ascii_general_ci");
                }

                //Indexes
                b.HasIndex(m => new { m.FolderId });
                b.HasIndex(m => new { m.Path }).IsUnique();
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<VirtualPathPermission>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "VirtualPathPermissions",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.ProviderName).IsRequired().HasMaxLength(VirtualPathConsts.ProviderNameMaxLength);
                b.Property(m => m.ProviderKey).HasMaxLength(VirtualPathConsts.ProviderKeyMaxLength);
                b.Property(m => m.CanRead).IsRequired().HasDefaultValue(false);
                b.Property(m => m.CanWrite).IsRequired().HasDefaultValue(false);
                b.Property(m => m.CanDelete).IsRequired().HasDefaultValue(false);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.ProviderName).UseCollation("ascii_general_ci");
                    b.Property(m => m.ProviderKey).UseCollation("ascii_general_ci");
                }

                //Indexes
                b.HasIndex(m => new { m.TargetId, m.ProviderName, m.ProviderKey });
                b.ApplyObjectExtensionMappings();
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