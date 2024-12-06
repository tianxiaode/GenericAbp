using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Exceptions;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.ParticipantIsolationFolders.Dtos;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ParticipantIsolationFolders;

public class ParticipantIsolationFolderAppService(
    ResourceManager resourceManager,
    FileInfoBaseManager fileInfoBaseManager)
    : FileManagementAppService, IParticipantIsolationFolderAppService
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    [Authorize]
    public virtual async Task<ResourceBaseDto> GetAsync(string folderName, Guid id)
    {
        var folder = await GetFolderAsync(folderName);
        var canRead = await ResourceManager.CanReadAsync(folder, CurrentUser.Id);
        var entity = await ResourceManager.GetAsync(id, folder.Id);
        if (canRead)
        {
            return ObjectMapper.Map<Resource, ResourceBaseDto>(entity);
        }

        //用户没有访问此文件夹的权限，只能返回属于自己的资源
        if (entity.OwnerId != CurrentUser.Id)
        {
            throw new EntityNotFoundBusinessException(L["File"], id);
        }

        return ObjectMapper.Map<Resource, ResourceBaseDto>(entity);
    }

    [Authorize]
    public virtual async Task<PagedResultDto<ResourceBaseDto>> GetListAsync(string folderName,
        GetParticipantIsolationFileInput input)
    {
        var folder = await GetFolderAsync(folderName);
        var queryParams = ObjectMapper.Map<GetParticipantIsolationFileInput, ResourceQueryParams>(input);
        queryParams.ParentId = folder.Id;
        var canRead = await ResourceManager.CanReadAsync(folder, CurrentUser.Id);
        if (!canRead)
        {
            //用户没有访问此文件夹的权限，只能获取属于自己的资源
            queryParams.OwnerId = CurrentUser.Id;
        }

        var predicate = await ResourceManager.BuildPredicateExpressionAsync(queryParams);
        var count = await ResourceManager.GetCountAsync(predicate);
        var list = await ResourceManager.GetPagedListAsync(predicate, queryParams);
        return new PagedResultDto<ResourceBaseDto>(count,
            ObjectMapper.Map<List<Resource>, List<ResourceBaseDto>>(list));
    }

    [Authorize]
    public virtual async Task<ICheckFileResult> CheckFileAsync(string folderName, FileCheckInput input)
    {
        var folder = await GetFolderAsync(folderName);
        await CheckFolderConfigurationAsync(folder, input.Size);
        return await FileInfoBaseManager.CheckAsync(input.Hash, input.Filename, folder.GetAllowedFileTypes(),
            input.Size, folder.GetMaxFileSize(), input.ChunkHash, input.ChunkSize, input.FirstChunk, long.MaxValue, 0);
    }

    [Authorize]
    public virtual async Task UploadChunkAsync(string folderName, FileUploadChunkInput input)
    {
        var folder = await GetFolderAsync(folderName);
        await CheckFolderConfigurationAsync(folder);
        await FileInfoBaseManager.UploadChunkAsync(input.Hash, input.ChunkBytes, input.Index, input.ChunkHash,
            folder.GetAllowedFileTypes());
    }

    public virtual async Task<FileDto> MergeAsync(string folderName, FileMergeInput input)
    {
        var folder = await GetFolderAsync(folderName);
        await CheckFolderConfigurationAsync(folder);
        var fileInfoBase = await FileInfoBaseManager.MergeAsync(input.Hash, input.Filename, input.TotalChunks,
            folder.GetAllowedFileTypes(), folder.GetMaxFileSize(), long.MaxValue, long.MaxValue);
        throw new NotImplementedException();
        // var folder = await CheckFolderPermission(input.FolderId, canWrite: true);
        // ;
        // var fileInfoBase = await FileInfoBaseManager.MergeAsync(input.Hash, input.TotalChunks,
        //     await folder.AllowedFileTypes.GetFileTypesAsync(), folder.MaxFileSize,
        //     folder.StorageQuota, folder.UsedStorage);
        // var entity = new File(GuidGenerator.Create(), folder.Id, input.Hash, CurrentTenant.Id);
        // var filename = await FileManager.GetFileNameAsync(folder.Id, input.Filename);
        // entity.Rename(filename);
        // entity.SetFileInfoBase(fileInfoBase);
        // await FileManager.CreateAsync(entity);
        // return ObjectMapper.Map<File, FileDto>(entity);
    }

    protected virtual async Task<Resource> GetFolderAsync(string folderName)
    {
        var root = await ResourceManager.GetParticipantIsolationsRootFolderAsync(CurrentTenant.Id);
        var folder = await ResourceManager.FindFolderByNameAsync(root.Id, folderName);
        return folder;
    }

    protected virtual async Task CheckFolderConfigurationAsync(Resource folder, long? fileSize = null)
    {
        var maxFileSize = folder.GetMaxFileSize();
        var fileCount = folder.GetAllowedFileCount();
        if (maxFileSize == -1 || fileCount == -1)
        {
            throw new FolderConfigurationNotSetBusinessException();
        }

        if (fileSize > maxFileSize)
        {
            throw new FileSizeOutOfRangeBusinessException(maxFileSize, fileSize);
        }

        var count = await ResourceManager.GetCountAsync(m => m.ParentId == folder.Id && m.OwnerId == CurrentUser.Id);
        if (count >= fileCount)
        {
            throw new FileCountLimitedBusinessException(fileCount);
        }
    }
}