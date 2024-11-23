using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : IRepository<Resource, Guid>
{
    /// <summary>
    /// Get all parent ids of a resource by code
    /// </summary>
    Task<List<Guid>> GetAllParentIdsAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all parent of a resource by code
    /// </summary>
    Task<List<Resource>> GetAllParentAsync(string code, CancellationToken cancellationToken = default);
}