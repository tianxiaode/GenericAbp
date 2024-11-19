using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.Extensions;
using Generic.Abp.Extensions.MimeDetective;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.Folders;
using Generic.Abp.FileManagement.Localization;
using Generic.Abp.FileManagement.Settings;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.FileInfoBases;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Generic.Abp.FileManagement.Files;

public class FileManager(
    IFileRepository repository,
    ICancellationTokenProvider cancellationTokenProvider,
    FilePermissionManager permissionManager,
    IdentityUserManager userManager,
    ISettingManager settingManager,
    IStringLocalizer<FileManagementResource> localizer,
    FileInfoBaseManager fileInfoBaseManager)
    : DomainService
{
    protected IFileRepository Repository { get; } = repository;
    protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
    protected CancellationToken CancellationToken => CancellationTokenProvider.Token;
    protected FilePermissionManager PermissionManager { get; } = permissionManager;
    protected IdentityUserManager UserManager { get; } = userManager;
    protected ISettingManager SettingManager { get; } = settingManager;
    protected IStringLocalizer<FileManagementResource> Localizer { get; } = localizer;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    public virtual Task<File> CreateAsync(File entity, bool autoSave = true)
    {
        return Repository.InsertAsync(entity, autoSave, CancellationToken);
    }

    public virtual Task<File> UpdateAsync(File entity, bool autoSave = true)
    {
        return Repository.UpdateAsync(entity, autoSave, CancellationToken);
    }

    public virtual Task DeleteAsync(File entity, bool autoSave = true)
    {
        return Repository.DeleteAsync(m => m.Id == entity.Id, autoSave, CancellationToken);
    }


    public virtual Task<File> GetAsync(Guid id)
    {
        return Repository.GetAsync(id, true, CancellationToken);
    }

    public virtual Task<File?> FindAsync(Expression<Func<File, bool>> predicate)
    {
        return Repository.FindAsync(predicate, true, CancellationToken);
    }

    public virtual async Task<File?> FindByHashAsync(string hash)
    {
        if (!hash.IsAscii())
        {
            return null;
        }

        return await Repository.FirstOrDefaultAsync(m => m.Hash.Equals(hash), CancellationToken);
    }

    public virtual async Task<bool> FileExistsAsync(Guid folderId, Guid fileInfoBaseId)
    {
        return await Repository.AnyAsync(m => m.FolderId == folderId && m.FileInfoBaseId == fileInfoBaseId,
            CancellationToken);
    }

    #region Filename

    public virtual async Task<string> GetFileNameAsync(Guid folderId, string filename)
    {
        var index = 1;
        while (await IsFileNameExistsAsync(folderId, filename))
        {
            filename = Path.GetFileNameWithoutExtension(filename) + "(" + index + ")" + Path.GetExtension(filename);
            index++;
        }

        return filename;
    }

    public virtual async Task<bool> IsFileNameExistsAsync(Guid? folderId, string filename)
    {
        return await Repository.AnyAsync(m => m.FolderId == folderId && m.Filename.ToLower() == filename.ToLower(),
            CancellationToken);
    }

    #endregion

    public virtual async Task<List<FilePermission>> GetPermissionsAsync(Guid id)
    {
        return await PermissionManager.GetListAsync(id, CancellationToken);
    }

    public virtual async Task SetPermissionsAsync(File entity, List<FilePermission> permissions)
    {
        var currentPermissions = await PermissionManager.GetListAsync(entity.Id, CancellationToken);

        var currentPermissionIds = new HashSet<Guid>(currentPermissions.Select(m => m.Id));
        var newPermissionIds = new HashSet<Guid>(permissions.Select(m => m.Id));

        // 找出需要删除的权限
        var removePermissions = currentPermissions.Where(m => !newPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.DeleteManyAsync(removePermissions, CancellationToken);

        // 找出需要新增的权限
        var insertPermissions = permissions.Where(m => !currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.InsertManyAsync(insertPermissions, CancellationToken);

        // 找出需要更新的权限
        var updatePermissions = permissions.Where(m => currentPermissionIds.Contains(m.Id)).ToList();
        await PermissionManager.UpdateManyAsync(updatePermissions, CancellationToken);
    }

    public virtual async Task<bool> CadReadAsync(File entity, Guid? userId)
    {
        //如果没有权限定义，则默认有读权限
        if (!await PermissionManager.HasPermissionAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (await PermissionManager.AllowEveryOneReadAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (!userId.HasValue)
        {
            return false;
        }

        if (await PermissionManager.AllowAuthenticatedUserReadAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesReadAsync(entity.Id, userId.Value, CancellationToken);
    }

    public virtual async Task<bool> CadWriteAsync(File entity, Guid? userId)
    {
        //如果没有权限定义，则默认有读权限
        if (!await PermissionManager.HasPermissionAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (await PermissionManager.AllowEveryOneWriteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (!userId.HasValue)
        {
            return false;
        }

        if (await PermissionManager.AllowAuthenticatedUserWriteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesWriteAsync(entity.Id, userId.Value, CancellationToken);
    }

    public virtual async Task<bool> CadDeleteAsync(File entity, Guid? userId)
    {
        //如果没有权限定义，则默认有读权限
        if (!await PermissionManager.HasPermissionAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (await PermissionManager.AllowEveryOneDeleteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        if (!userId.HasValue)
        {
            return false;
        }

        if (await PermissionManager.AllowAuthenticatedUserDeleteAsync(entity.Id, CancellationToken))
        {
            return true;
        }

        return await PermissionManager.AllowUserOrRolesDeleteAsync(entity.Id, userId.Value, CancellationToken);
    }

    public virtual async Task<bool> CheckPermissionAsync(File entity, Guid userId,
        Func<Guid, IList<string>, Expression<Func<FilePermission, bool>>, System.Threading.CancellationToken,
                Task<bool>>
            permissionCheckFunc)
    {
        Expression<Func<FilePermission, bool>> subPredicate = m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName;

        var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));

        //判断文件夹是否存在认证用户权限
        subPredicate = subPredicate.OrIfNotTrue(m =>
            m.ProviderName == FolderConsts.AuthorizationUserProviderName);

        //判断是否包含用户
        subPredicate.OrIfNotTrue(m =>
            m.ProviderName == FolderConsts.UserProviderName && m.ProviderKey == userId.ToString());
        return await permissionCheckFunc(entity.Id, roles, subPredicate, CancellationToken);
    }
}