using Generic.Abp.Extensions.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Resources;

public partial class ResourceManager
{
    public virtual async Task<Resource> GetVirtualPathAsync(Guid id)
    {
        var root = await GetVirtualRootFolderAsync();
        var resource = await Repository.FindAsync(m => m.Id == id && m.ParentId == root.Id, true, CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(Localizer["Path"], id);
        }

        return resource;
    }

    public virtual async Task<Resource> FindVirtualPathAsync(string virtualPath)
    {
        var root = await GetVirtualRootFolderAsync();
        var resource = await Repository.FindAsync(
            m => m.ParentId == root.Id && string.Equals(virtualPath, m.Name, StringComparison.OrdinalIgnoreCase),
            cancellationToken: CancellationToken);
        if (resource == null)
        {
            throw new EntityNotFoundBusinessException(Localizer["Path"], virtualPath);
        }

        return resource;
    }

    public virtual async Task<Tuple<long, List<Resource>>> GetVirtualPathsAsync(string? filter, DateTime? startTime,
        DateTime? endTime, string? fileType, string? sorting = null, int maxResultCount = int.MaxValue,
        int skipCount = 0)
    {
        var root = await GetVirtualRootFolderAsync();
        sorting ??= $"{nameof(Resource.Name)}";
        var predicate = await Repository.BuildQueryExpressionAsync(root.Id, filter, startTime, endTime, fileType);
        var count = await Repository.GetCountAsync(predicate, CancellationToken);
        var resources =
            await Repository.GetListAsync(predicate, sorting, maxResultCount, skipCount, true, CancellationToken);
        return Tuple.Create(count, resources);
    }

    public virtual async Task<Resource> CreateVirtualPathAsync(string name, Guid folderId)
    {
        await ValidateIsPublicFolderAsync(folderId);
        var root = await GetVirtualRootFolderAsync();
        var entity = new Resource(GuidGenerator.Create(), name, ResourceType.VirtualPath, false,
            CurrentTenant.Id);
        entity.SetFolderId(folderId);
        entity.MoveTo(root.Id);
        await CreateAsync(entity);
        return entity;
    }

    public virtual async Task<Resource> UpdateVirtualPathAsync(Guid id, string name, Guid folderId)
    {
        await ValidateIsPublicFolderAsync(folderId);
        var entity = await GetVirtualPathAsync(id);
        entity.Rename(name);
        entity.SetFolderId(folderId);
        await UpdateAsync(entity);
        return entity;
    }


    public virtual async Task DeleteVirtualPathAsync(Guid id)
    {
        var entity = await GetVirtualPathAsync(id);
        await DeleteAsync(entity);
    }

    public virtual async Task<List<ResourcePermission>> GetVirtualPathPermissionAsync(Guid id)
    {
        var entity = await GetVirtualPathAsync(id);
        return await GetPermissionsAsync(entity);
    }

    public virtual async Task UpdateVirtualPathPermissionAsync(Guid id, List<ResourcePermission> permissions)
    {
        var entity = await GetVirtualPathAsync(id);
        await SetPermissionsAsync(entity, permissions);
    }
}