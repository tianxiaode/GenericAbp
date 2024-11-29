using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using ResourceManager = Generic.Abp.VirtualPaths.ResourceManager;

namespace Generic.Abp.FileManagement.Folders;

//[Authorize]
public class PersonalFolderAppService(
    ResourceManager resourceManager,
    IResourceRepository repository,
    FileInfoBaseManager fileInfoBaseInfoBaseManager,
    IdentityUserManager userManager)
    : FileManagementAppService, IPersonalFolderAppService
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IResourceRepository Repository { get; } = repository;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseInfoBaseManager;
    protected IdentityUserManager UserManager { get; } = userManager;

    public virtual async Task<ListResultDto<FolderDto>> GetRootFoldersAsync()
    {
        var folderDtos = new List<FolderDto>();
        var publicRoot = await ResourceManager.GetPublicRootFolderAsync();
        folderDtos.Add(new FolderDto(publicRoot.Id, publicRoot.Code, L[publicRoot.Name]));

        var usersRoot = await ResourceManager.GetUsersRootFolderAsync();
        folderDtos.Add(new FolderDto(usersRoot.Id, usersRoot.Code, L[usersRoot.Name]));

        var sharedRoot = await ResourceManager.GetSharedRootFolderAsync();
        folderDtos.Add(new FolderDto(sharedRoot.Id, sharedRoot.Code, L[sharedRoot.Name]));
        return new ListResultDto<FolderDto>(folderDtos);
    }

    public virtual async Task<FolderDto> GetAsync(Guid id)
    {
        var folder = await CheckFolderPermission(id, canRead: true);
        return ObjectMapper.Map<Resource, FolderDto>(folder);
    }

    public virtual async Task<ListResultDto<FolderDto>> GetListAsync(FolderGetListInput input)
    {
        throw new NotImplementedException();
        // var userId = GetUserId();
        //
        // List<Resource> list = [];
        // if (!string.IsNullOrEmpty(input.Filter))
        // {
        //     var publicRootFolder = await ResourceManager.GetPublicRootFolderAsync();
        //     var sharedRootFolder = await ResourceManager.GetSharedRootFolderAsync();
        //     var userRootFolder = await ResourceManager.GetUserRootFolderAsync(userId);
        //     var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
        //     list = await Repository.GetCanReadFoldersForUserAsync(m => m.Name.Contains(input.Filter),
        //         publicRootFolder.Code, sharedRootFolder.Code, userRootFolder.Code, userId,
        //         roles.ToList());
        // }
        // else if (input.FolderId.HasValue)
        // {
        //     var folder = await CheckFolderPermission(input.FolderId.Value, canRead: true);
        //     list = await Repository.GetListAsync(m => m.ParentId == folder.Id);
        // }
        //
        // return new ListResultDto<FolderDto>(ObjectMapper.Map<List<Folder>, List<FolderDto>>(list));
    }

    #region Files

    public virtual async Task<PagedResultDto<FileDto>> GetFilesAsync(FileGetListInput input)
    {
        throw new NotImplementedException();
        // var userId = GetUserId();
        // var predicate = await BuildFilePredicate(input);
        // var publicRootFolder = await ResourceManager.GetPublicRootFolderAsync();
        // var sharedRootFolder = await ResourceManager.GetSharedRootFolderAsync();
        // var userRootFolder = await ResourceManager.GetUserRootFolderAsync(userId);
        // var roles = await UserManager.GetRolesAsync(await UserManager.GetByIdAsync(userId));
        // var count = await Repository.GetCanReadFilesCountForUserAsync(predicate, publicRootFolder.Code,
        //     sharedRootFolder.Code, userRootFolder.Code, userId, roles.ToList());
        // var files = await Repository.GetCanReadFilesForUserAsync(predicate, publicRootFolder.Code,
        //     sharedRootFolder.Code,
        //     userRootFolder.Code, userId, roles.ToList(), input.Sorting, input.SkipCount, input.MaxResultCount);
        // return new PagedResultDto<FileDto>(count, ObjectMapper.Map<List<File>, List<FileDto>>(files));
    }

    #endregion

    protected virtual async Task CheckFilePermission(Resource entity)
    {
        throw new NotImplementedException();
        // if (entity.IsInheritPermissions)
        // {
        //     return;
        // }
        //
        // if (await FileInfoBaseManager.CadReadAsync(entity, CurrentUser.Id))
        // {
        //     return;
        // }
        //
        // throw new AbpAuthorizationException(L["AccessDenied"]);
    }

    protected virtual async Task<Resource> CheckFolderPermission(Guid id, bool canRead = false, bool canWrite = false,
        bool canDelete = false)
    {
        var folder = await ResourceManager.GetAsync(id);
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        if (await ResourceManager.IsOwnerAsync(folder, CurrentUser.Id.Value))
        {
            return folder;
        }

        if (canWrite && await ResourceManager.IsRooFolderAsync(folder))
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        var can =
            canRead ? await ResourceManager.CadWriteAsync(folder, CurrentUser.Id.Value) :
            canWrite ? await ResourceManager.CadWriteAsync(folder, CurrentUser.Id.Value) :
            canDelete && await ResourceManager.CadDeleteAsync(folder, CurrentUser.Id.Value);
        if (can)
        {
            return folder;
        }

        throw new AbpAuthorizationException(L["AccessDenied"]);
    }

    protected virtual Task<Expression<Func<Resource, bool>>> BuildFilePredicate(FileGetListInput input)
    {
        Expression<Func<Resource, bool>> predicate = m => true;
        if (input.FolderId.HasValue)
        {
            predicate = predicate.AndIfNotTrue(m => m.FolderId == input.FolderId);
        }

        if (!string.IsNullOrEmpty(input.Filter))
        {
            predicate = predicate.AndIfNotTrue(m => m.Name.Contains(input.Filter));
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
            predicate = predicate.AndIfNotTrue(m => types.Contains(m.FileInfoBase.Extension));
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

    protected virtual Guid GetUserId()
    {
        if (!CurrentUser.Id.HasValue)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        return CurrentUser.Id.Value;
    }
}