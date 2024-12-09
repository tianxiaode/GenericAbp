using Generic.Abp.Extensions.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.Extensions.Trees;

public interface
    ITreeRepository<TEntity> : IExtensionRepository<TEntity>
    where TEntity : class, ITree<TEntity>
{
    Task<bool> HasChildAsync(Guid id, CancellationToken cancellation = default);

    Task<long> GetAllChildrenCountByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllChildrenByCodeAsync(string code,
        CancellationToken cancellationToken = default);

    Task<List<string>> GetAllCodesByFilterAsync(string filter,
        CancellationToken cancellationToken = default);

    Task<List<Guid>> GetAllParentIdsByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetAllParentByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<int> MoveByCodeAsync(string oldParentCode, string newParentCode,
        CancellationToken cancellationToken = default);

    Task<bool> HasParentChildConflictAsync(List<Guid> sourceIds,
        CancellationToken cancellation = default);

    Task<bool> HasParentChildConflictAsync(List<Guid> sourceIds, string? targetCode,
        CancellationToken cancellation = default);

    Task<int> DeleteAllChildrenByCodeAsync(string code, CancellationToken cancellationToken = default);
}