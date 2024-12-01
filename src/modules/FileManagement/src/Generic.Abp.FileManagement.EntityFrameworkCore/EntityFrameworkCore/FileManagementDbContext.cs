using Generic.Abp.FileManagement.ExternalShares;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.VirtualPaths;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    [ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
    public class FileManagementDbContext(DbContextOptions<FileManagementDbContext> options)
        : AbpDbContext<FileManagementDbContext>(options), IFileManagementDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */

        public DbSet<FileInfoBase> FileInfoBases { get; set; } = default!;
        public DbSet<Resource> Resources { get; set; } = default!;
        public DbSet<ResourcePermission> ResourcePermissions { get; set; } = default!;
        public DbSet<VirtualPath> VirtualPaths { get; set; } = default!;
        public DbSet<ExternalShare> ExternalShares { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFileManagement();
        }
    }
}