using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
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

        public DbSet<File> Files { get; set; } = default!;
        public DbSet<Folder> Folders { get; set; } = default!;
        public DbSet<VirtualPath> VirtualPaths { get; set; } = default!;
        public DbSet<FilePermission> FilePermissions { get; set; } = default!;
        public DbSet<FolderPermission> FolderPermissions { get; set; } = default!;
        public DbSet<VirtualPathPermission> VirtualPathPermissions { get; set; } = default!;
        public DbSet<FileInfoBase> FileInfoBases { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureFileManagement();
        }
    }
}