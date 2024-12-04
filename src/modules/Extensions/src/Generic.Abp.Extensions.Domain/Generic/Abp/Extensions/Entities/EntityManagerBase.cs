using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Generic.Abp.Extensions.Entities.IncludeOptions;
using Generic.Abp.Extensions.Entities.QueryParams;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;
using Microsoft.Extensions.Localization;

namespace Generic.Abp.Extensions.Entities;

public abstract class EntityManagerBase<TEntity, TRepository, TResource>(
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

    public virtual async Task<TEntity> GetAsync(Guid id)
    {
        return await Repository.GetAsync(id, false, CancellationToken);
    }

    public virtual async Task<TEntity> GetAsync(Guid id, IIncludeOptions includeOptions)
    {
        return await Repository.GetAsync(id, includeOptions, CancellationToken);
    }

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false)
    {
        return await Repository.FindAsync(predicate, includeDetails, CancellationToken);
    }

    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate,
        IIncludeOptions includeOption)
    {
        return await Repository.FindAsync(predicate, includeOption, CancellationToken);
    }

    public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Repository.GetCountAsync(predicate, CancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Repository.GetListAsync(predicate, false, CancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sorting,
        IIncludeOptions? includeOptions = null)
    {
        return await Repository.GetListAsync(predicate, sorting, includeOptions, CancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate,
        string sorting,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        IIncludeOptions? includeOptions = null)
    {
        return await Repository.GetPagedListAsync(predicate, sorting, skipCount, maxResultCount, includeOptions,
            CancellationToken);
    }

    public async Task<List<TEntity>> GetPagedListAsync(
        Expression<Func<TEntity, bool>> predicate,
        BaseQueryParams queryParams,
        IIncludeOptions? includeOptions = null)
    {
        return await Repository.GetPagedListAsync(predicate, queryParams, includeOptions, CancellationToken);
    }

    public virtual Task ValidateAsync(TEntity entity)
    {
        return Task.CompletedTask;
    }

    public virtual Task<Expression<Func<TEntity, bool>>> BuildPredicateExpression(BaseQueryParams queryParams)
    {
        throw new NotImplementedException();
    }
}