using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;

namespace Generic.Abp.FileManagement.Folders;

//[Authorize]
public class PersonalFolderAppService(
    FolderManager folderManager,
    IFolderRepository repository,
    FileManager fileManager,
    IdentityUserManager userManager)
    : FileManagementAppService, IPersonalFolderAppService
{
    protected FolderManager FolderManager { get; } = folderManager;
    protected IFolderRepository Repository { get; } = repository;
    protected FileManager FileManager { get; } = fileManager;
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<ListResultDto<FolderDto>> GetRootFoldersAsync()
    {
        var folderDtos = new List<FolderDto>();
        var publicRoot = await FolderManager.GetPublicRootFolderAsync();
        folderDtos.Add(new FolderDto(publicRoot.Id, publicRoot.Code, L[publicRoot.Name]));

        var usersRoot = await FolderManager.GetUsersRootFolderAsync();
        folderDtos.Add(new FolderDto(usersRoot.Id, usersRoot.Code, L[usersRoot.Name]));

        var sharedRoot = await FolderManager.GetSharedRootFolderAsync();
        folderDtos.Add(new FolderDto(sharedRoot.Id, sharedRoot.Code, L[sharedRoot.Name]));
        return new ListResultDto<FolderDto>(folderDtos);
    }

    public virtual async Task<FolderDto> GetAsync(Guid id)
    {
        var folder = await CheckFolderPermission(id, canRead: true);
        return ObjectMapper.Map<Folder, FolderDto>(folder);
    }

    public virtual async Task<ListResultDto<FolderDto>> GetListAsync(FolderGetListInput input)
    {
        var userId = new Guid("3a16576b-3492-9ac7-49f8-15f80c48bafa");
        // if (!CurrentUser.Id.HasValue)
        // {
        //     throw new AbpAuthorizationException(L["AccessDenied"]);
        // }

        List<Folder> list = [];
        if (!string.IsNullOrEmpty(input.Filter))
        {
            var publicRootFolder = await FolderManager.GetPublicRootFolderAsync();
            var sharedRootFolder = await FolderManager.GetSharedRootFolderAsync();
            var userRootFolder = await FolderManager.GetUserRootFolderAsync(userId);
            var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
            list = await Repository.GetCanReadFoldersForUserAsync(m => m.Name.Contains(input.Filter),
                publicRootFolder.Code, sharedRootFolder.Code, userRootFolder.Code, userId,
                roles.ToList());
        }
        else if (input.FolderId.HasValue)
        {
            var folder = await CheckFolderPermission(input.FolderId.Value, canRead: true);
            list = await Repository.GetListAsync(m => m.ParentId == folder.Id);
        }

        return new ListResultDto<FolderDto>(ObjectMapper.Map<List<Folder>, List<FolderDto>>(list));
    }

    #region Files

    public virtual async Task<PagedResultDto<FileDto>> GetFilesAsync(FileGetListInput input)
    {
        var userId = new Guid("3a16576b-3492-9ac7-49f8-15f80c48bafa");
        var predicate = await BuildFilePredicate(input);
        var publicRootFolder = await FolderManager.GetPublicRootFolderAsync();
        var sharedRootFolder = await FolderManager.GetSharedRootFolderAsync();
        var userRootFolder = await FolderManager.GetUserRootFolderAsync(userId);
        var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
        var count = await Repository.GetCanReadFilesCountForUserAsync(predicate, publicRootFolder.Code,
            sharedRootFolder.Code, userRootFolder.Code, userId, roles.ToList());
        var files = await Repository.GetCanReadFilesForUserAsync(predicate, publicRootFolder.Code,
            sharedRootFolder.Code,
            userRootFolder.Code, userId, roles.ToList(), input.Sorting, input.SkipCount, input.MaxResultCount);
        return new PagedResultDto<FileDto>(count, ObjectMapper.Map<List<File>, List<FileDto>>(files));
    }

    #endregion

    protected virtual async Task CheckFilePermission(File entity)
    {
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        if (entity.IsInheritPermissions)
        {
            return;
        }

        if (await FileManager.CadReadAsync(entity, CurrentUser.Id.Value))
        {
            return;
        }

        throw new AbpAuthorizationException(L["AccessDenied"]);
    }

    protected virtual async Task<Folder> CheckFolderPermission(Guid id, bool canRead = false, bool canWrite = false,
        bool canDelete = false)
    {
        var folder = await FolderManager.GetAsync(id);
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        if (await FolderManager.IsOwnerAsync(folder, CurrentUser.Id.Value))
        {
            return folder;
        }

        if (canWrite && await FolderManager.IsRooFolderAsync(folder))
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        var can =
            canRead ? await FolderManager.CadWriteAsync(folder, CurrentUser.Id.Value) :
            canWrite ? await FolderManager.CadWriteAsync(folder, CurrentUser.Id.Value) :
            canDelete && await FolderManager.CadDeleteAsync(folder, CurrentUser.Id.Value);
        if (can)
        {
            return folder;
        }

        throw new AbpAuthorizationException(L["AccessDenied"]);
    }

    protected virtual Task<Expression<Func<File, bool>>> BuildFilePredicate(FileGetListInput input)
    {
        Expression<Func<File, bool>> predicate = m => true;
        if (input.FolderId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FolderId == input.FolderId);
        }

        if (!string.IsNullOrEmpty(input.Filter))
        {
            predicate = predicate.AndIfNotTrue(m => m.Filename.Contains(input.Filter));
        }

        if (input.StartTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime >= input.StartTime);
        }

        if (input.EndTime.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.CreationTime <= input.EndTime);
        }

        if (!input.FileTypes.IsNullOrEmpty())
        {
            var types = input.FileTypes.Split(',').Select(m => m.Trim().ToLower()).ToList();
            predicate = predicate.AndIfNotTrue(m => types.Contains(m.FileInfoBase.FileType));
        }

        if (input.MinSize.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FileInfoBase.Size >= input.MinSize);
        }

        if (input.MaxSize.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FileInfoBase.Size <= input.MinSize);
        }

        return Task.FromResult(predicate);
    }
}