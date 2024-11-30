using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Exceptions;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.FileManagement.Resources;

/// <summary>
/// 管理与文件夹相关的资源
/// </summary>
public partial class ResourceManager
{
    public virtual async Task<Resource> CreateFolderAsync(string name, Guid parentId, string? allowFileTypes,
        string? quota, string? maxFileSize, Guid? tenantId = null)
    {
        await ValidateIsPublicOrIsSharedFolderAsync(parentId);

        var entity = new Resource(GuidGenerator.Create(), name, ResourceType.Folder, false, tenantId);
        entity.MoveTo(parentId);
        // if (!string.IsNullOrEmpty(allowFileTypes))
        // {
        //     entity.SetAllowedFileTypes(allowFileTypes);
        // }
        //
        // if (!string.IsNullOrEmpty(quota))
        // {
        //     entity.SetQuota(quota);
        // }
        //
        // if (!string.IsNullOrEmpty(maxFileSize))
        // {
        //     entity.SetMaxFileSize(maxFileSize);
        // }

        await CreateAsync(entity);
        return entity;
    }

    public virtual async Task<Resource> UpdateFolderAsync(Guid id, string name, string? allowFileTypes, string? quota,
        string? maxFileSize)
    {
        var entity = await Repository.GetAsync(id);
        if (!entity.ParentId.HasValue)
        {
            if (!await IsPublicFolderAsync(entity) || !await IsSharedFolderAsync(entity))
            {
                throw new EntityNotFoundBusinessException(Localizer["Folder"], name);
            }
        }
        else
        {
            await ValidateIsPublicOrIsSharedFolderAsync(entity.ParentId.Value);
        }


        entity.Rename(name);
        // if (!string.IsNullOrEmpty(allowFileTypes))
        // {
        //     entity.SetAllowedFileTypes(allowFileTypes);
        // }
        //
        // if (!string.IsNullOrEmpty(quota))
        // {
        //     entity.SetQuota(quota);
        // }
        //
        // if (!string.IsNullOrEmpty(maxFileSize))
        // {
        //     entity.SetMaxFileSize(maxFileSize);
        // }

        await UpdateAsync(entity);
        return entity;
    }

    public virtual async Task MoveFolderAsync(Guid id, Guid parentId)
    {
        if (parentId == id)
        {
            throw new CanNotMoveOrCopyToItselfBusinessException();
        }

        var entity = await Repository.GetAsync(id);

        if (!await IsPublicFolderAsync(entity))
        {
            throw new OnlyMovePublicFolderBusinessException();
        }

        ValidateIsStaticFolder(entity);

        var parent = await Repository.GetAsync(parentId);
        if (parent.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(Localizer["Folder"], parentId);
        }

        if (parent.Code.StartsWith(entity.Code))
        {
            throw new CanNotMoveToChildBusinessException();
        }

        if (!await IsPublicFolderAsync(parent))
        {
            throw new OnlyMovePublicFolderBusinessException();
        }

        await MoveAsync(entity, parentId);
    }

    public virtual async Task CopyFolderAsync(Guid id, Guid parentId)
    {
        if (parentId == id)
        {
            throw new CanNotMoveOrCopyToItselfBusinessException();
        }

        var entity = await Repository.GetAsync(id);
        if (entity.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(Localizer["Folder"], parentId);
        }

        var maxNodeCount = await SettingManager.GetFolderCopyMaxNodeCountAsync();
        var count = await Repository.GetAllChildrenCountByCodeAsync(entity.Code, CancellationToken);
        if (count >= maxNodeCount)
        {
            throw new OnlyMoveMaxFilesAndFoldersInOnTimeBusinessException(maxNodeCount, count);
        }

        var parent = await Repository.GetAsync(parentId);
        if (parent.Type != ResourceType.Folder)
        {
            throw new EntityNotFoundBusinessException(Localizer["Folder"], parentId);
        }

        await CopyAsync(entity, parentId);
    }

    public virtual async Task DeleteFolderAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id);
        ValidateIsStaticFolder(entity);
        await DeleteAsync(entity);
    }

    public virtual async Task<List<Resource>> GetSearchFoldersAsync(string filter)
    {
        return await Repository.GetListAsync(m => m.Name.Contains(filter) && m.Type == ResourceType.Folder, true,
            CancellationToken);
    }

    public virtual async Task<List<Resource>> GetChildrenFoldersAsync(Guid parentId)
    {
        return await Repository.GetListAsync(m => m.ParentId == parentId && m.Type == ResourceType.Folder, true,
            CancellationToken);
    }
}