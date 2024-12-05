using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Exceptions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Resources;

/// <summary>
/// Validation methods for <see cref="ResourceManager"/>
/// </summary>
public partial class ResourceManager
{
    public virtual Task<bool> IsRooFolderAsync(Resource entity)
    {
        return Task.FromResult(entity.ParentId == null);
    }

    public virtual async Task<bool> IsPublicFolderAsync(Guid id)
    {
        var publicRoot = await GetPublicRootFolderAsync();
        return await Repository.AnyAsync(m => m.Id == id && m.Code.StartsWith(publicRoot.Code), CancellationToken);
    }

    public virtual async Task<bool> IsPublicFolderAsync(Resource entity)
    {
        var publicRoot = await GetPublicRootFolderAsync();
        return entity.Code.StartsWith(publicRoot.Code);
    }

    public virtual async Task ValidateIsPublicFolderAsync(Guid folderId)
    {
        if (!await IsPublicFolderAsync(folderId))
        {
            throw new EntityNotFoundBusinessException(L["Folder"], folderId);
        }
    }


    public virtual async Task<bool> IsUsersFolderAsync(Guid id)
    {
        var publicRoot = await GetUsersRootFolderAsync();
        return await Repository.AnyAsync(m => m.Id == id && m.Code.StartsWith(publicRoot.Code), CancellationToken);
    }

    public virtual async Task<bool> IsUsersFolderAsync(Resource entity)
    {
        var privateRoot = await GetUsersRootFolderAsync();
        return entity.Code.StartsWith(privateRoot.Code);
    }

    public virtual async Task<bool> IsSharedFolderAsync(Guid id)
    {
        var publicRoot = await GetSharedRootFolderAsync();
        return await Repository.AnyAsync(m => m.Id == id && m.Code.StartsWith(publicRoot.Code), CancellationToken);
    }

    public virtual async Task ValidateIsPublicOrIsSharedFolderAsync(Guid folderId)
    {
        if (!await IsPublicFolderAsync(folderId) || !await IsSharedFolderAsync(folderId))
        {
            throw new EntityNotFoundBusinessException(L["Folder"], folderId);
        }
    }


    public virtual async Task<bool> IsSharedFolderAsync(Resource entity)
    {
        var sharedRoot = await GetSharedRootFolderAsync();
        return entity.Code.StartsWith(sharedRoot.Code);
    }


    public virtual async Task<bool> IsOwnerAsync(Resource entity, Guid userId)
    {
        var folderName = await GetRootFolderNameAsync(ResourceType.Folder, CurrentTenant.Id, userId);
        var codeLength = ResourceConsts.GetCodeLength(2);
        if (entity.Code.Length <= codeLength)
        {
            return entity.Name == folderName;
        }

        var parent = await Repository.GetAsync(m => m.Code == entity.Code.Substring(0, codeLength));
        return parent.Name == folderName;
    }

    public virtual void ValidateIsStaticFolder(Resource entity)
    {
        if (entity.IsStatic)
        {
            throw new StaticFolderCanNotBeMoveOrDeletedBusinessException();
        }
    }

    public virtual async Task<bool> IsExistsAsync(Guid id, Guid parentId)
    {
        var parent = await Repository.GetAsync(parentId);
        var entity = await Repository.GetAsync(id);
        return entity.Code.StartsWith(parent.Code);
    }
}