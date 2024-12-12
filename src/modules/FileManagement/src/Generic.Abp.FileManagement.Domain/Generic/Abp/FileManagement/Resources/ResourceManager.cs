using Generic.Abp.Extensions.Entities.QueryParams;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Events;
using Medallion.Threading;
using Microsoft.Extensions.Logging;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Threading;

namespace Generic.Abp.FileManagement.Resources;

public partial class ResourceManager(
    IResourceRepository repository,
    ITreeCodeGenerator<Resource> treeCodeGenerator,
    IStringLocalizer<FileManagementResource> localizer,
    IDistributedCache<ResourceCacheItem, string> cache,
    ResourcePermissionManager resourcePermissionManager,
    FileManagementSettingManager settingManager,
    IDistributedEventBus distributedEventBus,
    ICancellationTokenProvider cancellationTokenProvider,
    IDistributedLockProvider distributedLockProvider)
    : TreeManager<Resource, IResourceRepository, FileManagementResource>(repository, treeCodeGenerator, localizer,
        cancellationTokenProvider)
{
    protected IDistributedCache<ResourceCacheItem, string> Cache { get; } = cache;
    protected ResourcePermissionManager PermissionManager { get; } = resourcePermissionManager;
    protected FileManagementSettingManager SettingManager { get; } = settingManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;
    protected IDistributedLockProvider DistributedLockProvider { get; } = distributedLockProvider;

    public virtual async Task<Resource> GetAsync(Guid id, Guid parentId)
    {
        var entity = await FindAsync(m => m.Id == id && m.ParentId == parentId);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(Resource), id);
        }

        return entity;
    }

    public virtual async Task DeleteManyAsync(List<Guid> ids, Guid parentId,
        Guid? tenantId = null, bool needUpdateUsedStorage = true)
    {
        await base.DeleteManyAsync(ids);
        if (needUpdateUsedStorage)
        {
            await DistributedEventBus.PublishAsync(new ResourceDeletedEto()
            {
                ResourceIds = ids,
                ParentId = parentId,
                TenantId = CurrentTenant.Id,
            });
        }
    }

    public virtual async Task<List<Resource>> GetListByPermissionAsync(ResourceQueryParams queryParams, Guid userId,
        ResourcePermissionType permissionType)
    {
        var predicate = await BuildPredicateExpressionAsync(queryParams);
        var list = await Repository.GetChildrenByPermissionAsync(predicate, queryParams, userId,
            await PermissionManager.GetRolesAsync(userId), permissionType, CancellationToken);
        return list;
    }

    public override Task<Expression<Func<Resource, bool>>> BuildPredicateExpressionAsync(BaseQueryParams queryParams)
    {
        Expression<Func<Resource, bool>> predicate = m => true;
        if (queryParams is not ResourceQueryParams query)
        {
            return Task.FromResult(predicate);
        }

        predicate = m => m.ParentId == query.ParentId;

        if (query.ResourceType.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.Type == query.ResourceType.Value);
        }

        if (!string.IsNullOrEmpty(query.Filter))
        {
            predicate = predicate.AndIfNotTrue(m => m.Name.Contains(query.Filter));
        }

        if (query.StartTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime >= query.StartTime.Value);
        }

        if (query.EndTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime <= query.EndTime.Value);
        }

        if (query.OwnerId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.OwnerId == query.OwnerId);
        }

        if (query.MaxFileSize.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FileSize <= query.MaxFileSize.Value);
        }

        if (query.MinFileSize.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FileSize >= query.MinFileSize.Value);
        }


        if (string.IsNullOrEmpty(query.FileType))
        {
            return Task.FromResult(predicate);
        }


        var types = query.FileType.Split(",");
        predicate = PredicateExtensions.And(predicate, m => types.Contains(m.FileExtension));

        return Task.FromResult(predicate);
    }
}