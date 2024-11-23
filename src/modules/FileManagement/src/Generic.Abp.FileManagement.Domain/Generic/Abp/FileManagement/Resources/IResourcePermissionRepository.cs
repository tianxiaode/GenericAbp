using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourcePermissionRepository : IRepository<ResourcePermission, Guid>
{
}