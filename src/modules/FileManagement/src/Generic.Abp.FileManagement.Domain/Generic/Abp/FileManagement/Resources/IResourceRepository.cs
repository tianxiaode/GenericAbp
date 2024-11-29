using Generic.Abp.Extensions.Trees;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : ITreeRepository<Resource>
{
    Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code, CancellationToken cancellationToken);

    Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? filetype);
}