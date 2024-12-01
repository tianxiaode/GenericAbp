using System;
using Generic.Abp.FileManagement.ExternalShares;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.VirtualPaths;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.MultiTenancy;

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
                b.HasIndex(m => m.TenantId);
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.Hash).IsUnique();
                b.HasIndex(m => m.Extension);
                b.HasIndex(m => new { m.ExpireAt, m.RetentionPolicy });
            });

            builder.Entity<Resource>(b =>
            {
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "Resources",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Name).IsRequired().HasMaxLength(ResourceConsts.NameMaxLength);
                b.Property(m => m.Code).IsRequired().HasMaxLength(ResourceConsts.CodeMaxLength);
                b.Property(m => m.IsAccessible).IsRequired().HasDefaultValue(true);
                b.Property(m => m.IsStatic).IsRequired().HasDefaultValue(true);
                b.Property(m => m.HasConfiguration).IsRequired().HasDefaultValue(false);
                b.Property(m => m.HasPermissions).IsRequired().HasDefaultValue(false);

                if (dbType == EfCoreDatabaseProvider.MySql)
                {
                    b.Property(m => m.Name).UseCollation("gbk_chinese_ci");
                }

                //Relations
                b.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId)
                    .IsRequired(false);
                b.HasOne(m => m.FileInfoBase).WithMany().HasForeignKey(m => m.FileInfoBaseId).IsRequired(false);
                b.HasMany(r => r.Permissions).WithOne(p => p.Resource).HasForeignKey(p => p.ResourceId);

                //Indexes
                b.HasIndex(m => new { m.TenantId, m.Name });
                b.HasIndex(m => new { m.TenantId, m.Code }).IsUnique();
                b.HasIndex(m => new { m.TenantId, m.CreationTime });
                b.HasIndex(m => new { m.TenantId, m.ParentId });
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.FileInfoBaseId);
                b.HasIndex(m => m.OwnerId);

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
                b.HasIndex(m => new { m.TenantId, m.ResourceId });
                b.HasIndex(m => new { m.TenantId, m.ResourceId, m.ProviderName, m.ProviderKey });
            });

            builder.Entity<VirtualPath>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "VirtualPaths",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.Name).IsRequired().HasMaxLength(VirtualPathConsts.NameMaxLength);
                b.Property(m => m.IsAccessible).IsRequired().HasDefaultValue(true);

                //Relations
                b.HasOne(m => m.Resource).WithMany().HasForeignKey(m => m.ResourceId)
                    .IsRequired(true);

                //Indexes
                b.HasIndex(m => m.TenantId);
                b.HasIndex(m => m.Name);
                b.HasIndex(m => m.CreationTime);
            });

            builder.Entity<ExternalShare>(b =>
            {
                //Configure table & schema name
                b.ToTable(FileManagementDbProperties.DbTablePrefix + "ExternalShares",
                    FileManagementDbProperties.DbSchema);

                b.ConfigureByConvention();

                //Properties
                b.Property(m => m.LinkName).IsRequired().HasMaxLength(ExternalShareConsts.LinkNameMaxLength);
                b.Property(m => m.Password).IsRequired().HasMaxLength(ExternalShareConsts.PasswordMaxLength);
                b.Property(m => m.ExpireTime).IsRequired().HasDefaultValue(DateTime.Now);

                //Relations
                b.HasOne(m => m.Resource).WithMany().HasForeignKey(m => m.ResourceId)
                    .IsRequired(true);

                //Indexes
                b.HasIndex(m => m.TenantId);
                b.HasIndex(m => m.LinkName);
                b.HasIndex(m => m.CreationTime);
                b.HasIndex(m => m.ExpireTime);
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