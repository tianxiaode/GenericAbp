using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathPermissionRepository : IRepository<VirtualPathPermission, Guid>
{
}