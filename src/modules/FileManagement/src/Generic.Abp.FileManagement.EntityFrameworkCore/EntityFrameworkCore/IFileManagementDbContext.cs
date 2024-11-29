using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
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
        DbSet<FileInfoBase> FileInfoBases { get; }
        DbSet<Resource> Resources { get; }
        DbSet<ResourcePermission> ResourcePermissions { get; }
        DbSet<ResourceConfiguration> ResourceConfigurations { get; }
    }
}