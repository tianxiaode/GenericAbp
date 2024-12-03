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
    ICancellationTokenProvider cancellationTokenProvider,
    IResourceConfigurationRepository configurationRepository)
    : TreeManager<Resource, IResourceRepository, FileManagementResource>(repository, treeCodeGenerator, localizer,
        cancellationTokenProvider)
{
    protected IDistributedCache<ResourceCacheItem, string> Cache { get; } = cache;
    protected ResourcePermissionManager PermissionManager { get; } = resourcePermissionManager;
    protected FileManagementSettingManager SettingManager { get; } = settingManager;
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;
    protected IResourceConfigurationRepository ConfigurationRepository { get; } = configurationRepository;

    public virtual async Task<Resource> GetAsync(Guid id, ResourceQueryOptions options)
    {
        var entity = await Repository.GetAsync(id, null, options, CancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundBusinessException(L["Folder"], id);
        }

        return entity;
    }

    public virtual async Task<Resource?> FindAsync(Expression<Func<Resource, bool>> predicate, Guid? parentId,
        ResourceQueryOptions options)
    {
        return await Repository.FindAsync(predicate, null, options, CancellationToken);
    }

    public override async Task DeleteAsync(Resource entity, bool autoSave = true)
    {
        await SetPermissionsAsync(entity, []);
        entity.SetHasPermissions(false);

        await base.DeleteAsync(entity, autoSave);
    }

    public override async Task ValidateAsync(Resource entity)
    {
        if (await Repository.AnyAsync(m =>
                m.ParentId == entity.ParentId && m.Id != entity.Id &&
                m.Name.ToLower() == entity.Name.ToLowerInvariant()))
        {
            throw new DuplicateWarningBusinessException(L[nameof(Resource)], entity.Name);
        }
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

    protected virtual Task CreateOrUpdateConfigurationAsync(Resource entity, string allowFileTypes, long storageQuota,
        long maxFileSize)
    {
        if (entity.Configuration == null)
        {
            entity.SetConfiguration(new ResourceConfiguration(GuidGenerator.Create(), allowFileTypes, storageQuota,
                maxFileSize, entity.TenantId));
        }
        else
        {
            entity.Configuration.SetAllowedFileTypes(allowFileTypes);
            entity.Configuration.SetStorageQuota(storageQuota);
            entity.Configuration.SetMaxFileSize(maxFileSize);
        }

        return Task.CompletedTask;
    }
}