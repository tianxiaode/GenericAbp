using Generic.Abp.Extensions.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.QueryOptions;
using Generic.Abp.Extensions.Entities.SearchParams;

namespace Generic.Abp.Extensions.Trees;

public interface
    ITreeRepository<TEntity, in TQueryOptions, in TSearchParams> : IExtensionRepository<TEntity, TQueryOptions,
    TSearchParams>
    where TEntity : class, ITree<TEntity>
    where TSearchParams : class, ISearchParams
    where TQueryOptions : QueryOption
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