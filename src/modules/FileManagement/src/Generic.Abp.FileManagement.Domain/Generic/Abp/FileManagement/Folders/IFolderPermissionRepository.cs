using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Folders;

public interface IFolderPermissionRepository : IRepository<FolderPermission, Guid>
{
}