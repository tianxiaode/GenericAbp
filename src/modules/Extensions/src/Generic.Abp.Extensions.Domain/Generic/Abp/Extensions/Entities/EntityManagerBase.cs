using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Generic.Abp.Extensions.Entities.GetListParams;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Extensions.Entities;

public class EntityManagerBase<TEntity, TRepository, TResource>(
    TRepository repository,
    IStringLocalizer<TResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider) : DomainService
    where TEntity : class, IEntity<Guid>
    where TRepository : IExtensionRepository<TEntity>
{
    protected TRepository Repository { get; } = repository;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;
    protected IStringLocalizer<TResource> L { get; } = localizer;

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

    public virtual async Task DeleteManyAsync(List<Guid> ids, bool autoSave = true)
    {
        await Repository.DeleteManyAsync(ids, autoSave, CancellationToken);
    }

    public virtual async Task DeleteManyAsync(List<TEntity> entities, bool autoSave = true)
    {
        await Repository.DeleteManyAsync(entities, autoSave, CancellationToken);
    }

    public virtual async Task<TEntity> GetAsync(Guid id, bool includeDetails = false)
    {
        return await Repository.GetAsync(id, includeDetails, CancellationToken);
    }

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false)
    {
        return await Repository.FindAsync(predicate, includeDetails, CancellationToken);
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

public class EntityManagerBase<TEntity, TRepository, TResource, TSearchParams>(
    TRepository repository,
    IStringLocalizer<TResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider)
    : EntityManagerBase<TEntity, TRepository, TResource>(repository, localizer, cancellationTokenProvider)
    where TEntity : class, IEntity<Guid>
    where TRepository : IExtensionRepository<TEntity, TSearchParams>
    where TSearchParams : class, ISearchParams
{
    public virtual async Task<Expression<Func<TEntity, bool>>> BuildPredicateExpression(TSearchParams searchParams)
    {
        return await Repository.BuildPredicateExpression(searchParams);
    }
}