using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;

namespace Generic.Abp.Extensions.Entities;

public class EntityManagerBase<TEntity, TRepository>(
    TRepository repository,
    ICancellationTokenProvider cancellationTokenProvider) : DomainService
    where TEntity : class, IEntity<Guid>
    where TRepository : IExtensionRepository<TEntity>
{
    protected TRepository Repository { get; } = repository;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;

    public virtual async Task CreateAsync(TEntity entity, bool autoSave = true)
    {
        await ValidateAsync(entity);
        await Repository.InsertAsync(entity, autoSave, cancellationToken: CancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true)
    {
        await ValidateAsync(entity);
        await Repository.UpdateAsync(entity, autoSave, cancellationToken: CancellationToken);
    }

    public virtual async Task DeleteAsync(Guid id, bool autoSave = true)
    {
        var entity = await Repository.GetAsync(id, false, CancellationToken);
        await DeleteAsync(entity, autoSave);
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = true)
    {
        await Repository.DeleteAsync(entity, true, CancellationToken);
    }

    public virtual async Task DeleteAsync(List<Guid> ids, bool autoSave = true)
    {
        await Repository.DeleteManyAsync(ids, autoSave, CancellationToken);
    }

    public virtual async Task DeleteAsync(List<TEntity> entities, bool autoSave = true)
    {
        await Repository.DeleteManyAsync(entities, autoSave, CancellationToken);
    }

    public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Repository.GetCountAsync(predicate, CancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> predicate,
        string sorting, int maxResultCount = int.MaxValue, int skipCount = 0,
        bool includeDetails = false)
    {
        return await Repository.GetListAsync(
            predicate,
            sorting,
            maxResultCount,
            skipCount,
            includeDetails,
            CancellationToken);
    }


    public virtual Task ValidateAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }
}