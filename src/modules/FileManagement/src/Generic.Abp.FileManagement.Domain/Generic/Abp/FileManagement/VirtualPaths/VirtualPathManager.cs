using System;
using System.Linq.Expressions;
using Generic.Abp.Extensions.Entities;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Localization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.QueryParams;
using Generic.Abp.Extensions.Extensions;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathManager(
    IVirtualPathRepository repository,
    IStringLocalizer<FileManagementResource> localizer,
    ICancellationTokenProvider cancellationTokenProvider,
    ILookupNormalizer normalizer)
    : EntityManagerBase<VirtualPath, IVirtualPathRepository, FileManagementResource>(
        repository, localizer,
        cancellationTokenProvider)
{
    protected ILookupNormalizer LookupNormalizer { get; } = normalizer;

    public virtual async Task<VirtualPath> FinByNameAsync(string name, bool includeDetails = true)
    {
        var entity = await FindAsync(m => string.Equals(m.NormalizedName, LookupNormalizer.NormalizeName(name)),
            includeDetails);
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["VirtualPath"], name);
        }

        return entity;
    }

    public override Task CreateAsync(VirtualPath entity, bool autoSave = true)
    {
        entity.SetNormalizedName(LookupNormalizer.NormalizeName(entity.Name));
        return base.CreateAsync(entity, autoSave);
    }

    public override Task UpdateAsync(VirtualPath entity, bool autoSave = true)
    {
        entity.SetNormalizedName(LookupNormalizer.NormalizeName(entity.Name));
        return base.UpdateAsync(entity, autoSave);
    }

    public virtual async Task CheckIsAccessibleAsync(VirtualPath entity)
    {
        if (!entity.IsAccessible || entity.Resource == null)
        {
            throw new EntityNotFoundBusinessException(L["VirtualPath"], entity.Name);
        }

        if (entity.StartTime > DateTime.UtcNow || entity.EndTime < DateTime.UtcNow)
        {
            entity.SetIsAccessible(false);
            await UpdateAsync(entity);
            throw new EntityNotFoundBusinessException(L["VirtualPath"], entity.Name);
        }
    }

    public override async Task ValidateAsync(VirtualPath entity)
    {
        if (await Repository.AnyAsync(
                m => string.Equals(m.NormalizedName, entity.NormalizedName) && m.Id != entity.Id,
                CancellationToken))
        {
            throw new DuplicateWarningBusinessException(L["VirtualPath"], entity.Name);
        }
    }

    public override Task<Expression<Func<VirtualPath, bool>>> BuildPredicateExpressionAsync(
        BaseQueryParams baseQueryParamsqueryParams)
    {
        var queryParams = baseQueryParamsqueryParams as VirtualPathQueryParams;
        Expression<Func<VirtualPath, bool>> predicate = (virtualPath) => true;
        if (queryParams == null)
        {
            return Task.FromResult(predicate);
        }

        if (!string.IsNullOrEmpty(queryParams.Filter))
        {
            predicate = predicate.OrIfNotTrue(m => m.Name.Contains(queryParams.Filter));
        }

        if (queryParams.StartTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime >= queryParams.StartTime);
        }

        if (queryParams.EndTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime <= queryParams.EndTime);
        }

        if (queryParams.IsAccessible.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.IsAccessible == queryParams.IsAccessible);
        }

        return Task.FromResult(predicate);
    }
}