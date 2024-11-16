using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathRepository : IRepository<VirtualPath, Guid>
{
}