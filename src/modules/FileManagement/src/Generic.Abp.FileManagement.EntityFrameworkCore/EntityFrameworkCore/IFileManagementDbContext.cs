using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.VirtualPaths;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore
{
    [ConnectionStringName(FileManagementDbProperties.ConnectionStringName)]
    public interface IFileManagementDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */

        DbSet<Generic.Abp.FileManagement.Files.File> Files { get; }
        DbSet<FilePermission> FilePermissions { get; }
        DbSet<Folder> Folders { get; }
        DbSet<FolderPermission> FolderPermissions { get; }
        DbSet<VirtualPath> VirtualPaths { get; }
        DbSet<VirtualPathPermission> VirtualPathPermissions { get; }
        DbSet<FolderFile> FolderFiles { get; }
    }
}