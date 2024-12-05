using Generic.Abp.Extensions.Entities.QueryParams;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Caching;
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
    ICancellationTokenProvider cancellationTokenProvider)
    : TreeManager<Resource, IResourceRepository, FileManagementResource>(repository, treeCodeGenerator, localizer,
        cancellationTokenProvider)
{
    protected IDistributedCache<ResourceCacheItem, string> Cache { get; } = cache;
    protected ResourcePermissionManager PermissionManager { get; } = resourcePermissionManager;
    protected FileManagementSettingManager SettingManager { get; } = settingManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;


    public override async Task DeleteAsync(Resource entity, bool autoSave = true)
    {
        await SetPermissionsAsync(entity, []);
        entity.SetHasPermissions(false);

        await base.DeleteAsync(entity, autoSave);
    }

    protected override Task<Resource> CloneAsync(Resource source)
    {
        var entity = new Resource(GuidGenerator.Create(), source.Name, ResourceType.Folder, source.IsStatic,
            source.TenantId);
        entity.SetFileInfoBase(source.FileInfoBaseId);

        //不复制配置，否则会导致配置副作用，譬如扩容
        entity.SetHasConfiguration(false);

        //不复制权限，否则会导致权限副作用，譬如扩权
        //entity.SetPermissions(source.Permissions);
        entity.SetHasPermissions(false);
        return Task.FromResult(entity);
    }

    public override Task<Expression<Func<Resource, bool>>> BuildPredicateExpressionAsync(BaseQueryParams queryParams)
    {
        Expression<Func<Resource, bool>> predicate = m => true;
        if (queryParams is not ResourceQueryParams query)
        {
            return Task.FromResult(predicate);
        }

        predicate = m => m.ParentId == query.ParentId;


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