using Generic.Abp.Extensions.Trees;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : ITreeRepository<Resource>
{
    Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code, CancellationToken cancellationToken);

    Task<long> SumSizeByCodeAsync(string code, List<Guid> ids,
        CancellationToken cancellationToken = default);

    Task<Resource> GetParentWithConfiguration(string code,
        CancellationToken cancellationToken = default);

    Task<List<Resource>> GetChildrenByPermissionAsync(
        Expression<Func<Resource, bool>> predicate,
        ResourceQueryParams queryParams,
        Guid userId,
        IList<string> roles,
        ResourcePermissionType permissionType,
        CancellationToken cancellationToken = default);
}