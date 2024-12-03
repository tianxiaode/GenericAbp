using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
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

    public virtual async Task<Resource> GetAsync(Guid id, ResourceQueryOption option)
    {
        var entity = await Repository.GetAsync(id, null, option, CancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], id);
        }

        return entity;
    }

    public virtual async Task<Resource?> FindAsync(Expression<Func<Resource, bool>> predicate, Guid? parentId,
        ResourceQueryOption option)
    {
        return await Repository.FindAsync(predicate, null, option, CancellationToken);
    }

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
}