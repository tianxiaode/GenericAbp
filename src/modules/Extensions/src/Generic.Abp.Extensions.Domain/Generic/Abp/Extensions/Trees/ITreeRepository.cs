using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.Extensions.Trees;

public interface ITreeRepository<TEntity> : IRepository<TEntity, Guid>
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

    Task<int> DeleteAllChildrenByCodeAsync(string code, CancellationToken cancellationToken = default);
}