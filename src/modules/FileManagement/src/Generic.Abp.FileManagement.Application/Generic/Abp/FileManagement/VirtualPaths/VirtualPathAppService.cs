using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.FileManagement.Resources.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Resource = Generic.Abp.FileManagement.Resources.Resource;


namespace Generic.Abp.FileManagement.VirtualPaths;

[RemoteService(false)]
public class VirtualPathAppService(
    ResourceManager resourceManager,
    IResourceRepository repository,
    FileInfoBaseManager fileInfoBaseManager)
    : FileManagementAppService, IVirtualPathAppService
{
    protected ResourceManager ResourceManager { get; } = resourceManager;
    protected IResourceRepository Repository { get; } = repository;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    [AllowAnonymous]
    public virtual async Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input)
    {
        var entity = await ResourceManager.FindVirtualPathAsync(path);

        //是否存在关联文件夹
        if (entity.Folder == null)
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        //是否有权限
        var canRead = await ResourceManager.CadReadAsync(entity, CurrentUser.Id);
        if (!canRead)
        {
            throw new AbpAuthorizationException(L["AccessDenied"]);
        }

        //文件是否存在
        var file = await FileInfoBaseManager.FindByHashAsync(hash);
        if (file == null)
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        //关联文件夹是否存在文件
        if (!await ResourceManager.FileExistsAsync(entity.Folder.Code, file.Id))
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        return await FileInfoBaseManager.GetFileAsync(file, input.ChunkSize, input.Index);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> GetAsync(Guid id)
    {
        var entity = await ResourceManager.GetVirtualPathAsync(id);

        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> FindByNameAsync(string name)
    {
        var entity = await ResourceManager.FindVirtualPathAsync(name);
        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<PagedResultDto<ResourceDto>> GetListAsync(VirtualPathGetListInput input)
    {
        var (count, list) = await ResourceManager.GetVirtualPathsAsync(input.Filter, input.StartTime, input.EndTime,
            input.FileType, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<ResourceDto>(count, ObjectMapper.Map<List<Resource>, List<ResourceDto>>(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Create)]
    public virtual async Task<ResourceDto> CreateAsync(VirtualPathCreateDto input)
    {
        var entity = await ResourceManager.CreateVirtualPathAsync(input.Name, input.FolderId);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Update)]
    public virtual async Task<ResourceDto> UpdateAsync(Guid id, VirtualPathUpdateDto input)
    {
        var entity = await ResourceManager.UpdateVirtualPathAsync(id, input.Name, input.FolderId);
        return ObjectMapper.Map<Resource, ResourceDto>(entity);
    }

    //[Authorize(FileManagementPermissions.VirtualPaths.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await ResourceManager.DeleteVirtualPathAsync(id);
    }

    #region Permission

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task<ListResultDto<ResourcePermissionDto>> GetPermissionsAsync(Guid id)
    {
        var list = await ResourceManager.GetVirtualPathPermissionAsync(id);
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
        await ResourceManager.UpdateVirtualPathPermissionAsync(id, permissions);
    }

    #endregion
}