using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Files;

public interface IFilePermissionRepository : IRepository<FilePermission, Guid>
{
}