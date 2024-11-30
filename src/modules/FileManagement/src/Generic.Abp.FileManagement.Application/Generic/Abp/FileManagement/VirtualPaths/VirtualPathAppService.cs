using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Generic.Abp.VirtualPaths;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Resource = Generic.Abp.FileManagement.Resources.Resource;


namespace Generic.Abp.FileManagement.VirtualPaths;

[RemoteService(false)]
public class VirtualPathAppService(
    VirtualPathManager virtualPathManager,
    FileInfoBaseManager fileInfoBaseManager)
    : FileManagementAppService, IVirtualPathAppService
{
    protected VirtualPathManager VirtualPathManager { get; } = virtualPathManager;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    #region 文件相关

    [AllowAnonymous]
    public virtual async Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input)
    {
        var entity = await VirtualPathManager.FindVirtualPathAsync(path);


        //是否有权限
        await CheckReadPermissionAsync(entity);

        var file = await GetAndValidateFileAssociationAsync(entity, hash);

        return await FileInfoBaseManager.GetFileAsync(file, input.ChunkSize, input.Index);
    }

    #endregion

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> GetAsync(Guid id)
    {
        var entity = await VirtualPathManager.GetVirtualPathAsync(id);

        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> FindByNameAsync(string name)
    {
        var entity = await VirtualPathManager.FindVirtualPathAsync(name);
        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<PagedResultDto<ResourceDto>> GetListAsync(VirtualPathGetListInput input)
    {
        var (count, list) = await VirtualPathManager.GetVirtualPathsAsync(input.Filter, input.StartTime, input.EndTime,
            input.FileType, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ResourceDto>(count, MapToResourceDtos(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Create)]
    public virtual async Task<ResourceDto> CreateAsync(VirtualPathCreateDto input)
    {
        var entity = await VirtualPathManager.CreateVirtualPathAsync(input.Name, input.FolderId);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Update)]
    public virtual async Task<ResourceDto> UpdateAsync(Guid id, VirtualPathUpdateDto input)
    {
        var entity = await VirtualPathManager.UpdateVirtualPathAsync(id, input.Name, input.FolderId);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await VirtualPathManager.DeleteVirtualPathAsync(id);
    }

    #region Permission

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task<ListResultDto<ResourcePermissionDto>> GetPermissionsAsync(Guid id)
    {
        var list = await VirtualPathManager.GetVirtualPathPermissionAsync(id);
        return new ListResultDto<ResourcePermissionDto>(
            ObjectMapper.Map<List<ResourcePermission>, List<ResourcePermissionDto>>(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task UpdatePermissionAsync(Guid id,
        ResourcePermissionsCreateOrUpdateDto input)
    {
        var permissions = input.Permissions.Select(m =>
            new ResourcePermission(m.Id ?? GuidGenerator.Create(), id, m.ProviderName, m.ProviderKey,
                m.Permissions, CurrentTenant.Id)).ToList();
        await VirtualPathManager.UpdateVirtualPathPermissionAsync(id, permissions);
    }

    #endregion

    protected virtual async Task<FileInfoBase> GetAndValidateFileAssociationAsync(Resource entity, string hash)
    {
        // 校验文件夹关联
        if (entity.Folder == null)
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 校验文件是否存在
        var file = await FileInfoBaseManager.FindByHashAsync(hash);
        if (file == null)
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 校验文件是否存在于文件夹中
        if (!await VirtualPathManager.FileExistsAsync(entity.Folder.Code, file.Id))
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 返回文件实例
        return file;
    }

    protected virtual async Task CheckReadPermissionAsync(Resource resource)
    {
        var canRead = await VirtualPathManager.CanReadAsync(resource, CurrentUser.Id);
        if (!canRead)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }
    }

    protected virtual async Task CheckWritePermissionAsync(Resource resource)
    {
        var canWrite = await VirtualPathManager.CanWriteAsync(resource, CurrentUser.Id);
        if (!canWrite)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }
    }
}