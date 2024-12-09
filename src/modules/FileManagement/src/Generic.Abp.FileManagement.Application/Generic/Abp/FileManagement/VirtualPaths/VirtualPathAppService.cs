using Generic.Abp.Extensions.Exceptions;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Resource = Generic.Abp.FileManagement.Resources.Resource;


namespace Generic.Abp.FileManagement.VirtualPaths;

[RemoteService(false)]
public class VirtualPathAppService(
    VirtualPathManager virtualPathManager,
    FileInfoBaseManager fileInfoBaseManager,
    ResourceManager resourceManager)
    : FileManagementAppService, IVirtualPathAppService
{
    protected VirtualPathManager VirtualPathManager { get; } = virtualPathManager;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;
    protected ResourceManager ResourceManager { get; } = resourceManager;

    #region 文件相关

    [AllowAnonymous]
    public virtual async Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input)
    {
        var entity = await VirtualPathManager.FinByNameAsync(path);

        await VirtualPathManager.CheckIsAccessibleAsync(entity);

        var file = await GetAndValidateFileAssociationAsync(entity.Resource, hash);

        return await FileInfoBaseManager.GetFileAsync(file, input.ChunkSize, input.Index);
    }

    #endregion

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> GetAsync(Guid id)
    {
        var entity = await VirtualPathManager.GetAsync(id, true);

        return ObjectMapper.Map<VirtualPath, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> FindByNameAsync(string name)
    {
        var entity = await VirtualPathManager.FinByNameAsync(name);
        return ObjectMapper.Map<VirtualPath, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<PagedResultDto<VirtualPathDto>> GetListAsync(VirtualPathGetListInput input)
    {
        var queryParams = ObjectMapper.Map<VirtualPathGetListInput, VirtualPathQueryParams>(input);
        var predicate = await VirtualPathManager.BuildPredicateExpressionAsync(queryParams);
        var count = await VirtualPathManager.GetCountAsync(predicate);
        var list = await VirtualPathManager.GetPagedListAsync(predicate, queryParams);
        return new PagedResultDto<VirtualPathDto>(count,
            ObjectMapper.Map<List<VirtualPath>, List<VirtualPathDto>>(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Create)]
    public virtual async Task<VirtualPathDto> CreateAsync(VirtualPathCreateDto input)
    {
        var entity = new VirtualPath(GuidGenerator.Create(), input.Name, input.ResourceId, input.IsAccessible,
            CurrentTenant.Id);
        await UpdateByInputAsync(entity, input);
        await VirtualPathManager.CreateAsync(entity);
        return ObjectMapper.Map<VirtualPath, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Update)]
    public virtual async Task<VirtualPathDto> UpdateAsync(Guid id, VirtualPathUpdateDto input)
    {
        var entity = await VirtualPathManager.GetAsync(id);
        entity.Rename(input.Name);
        entity.ChangeResource(input.ResourceId);
        await UpdateByInputAsync(entity, input);
        await VirtualPathManager.UpdateAsync(entity);
        return ObjectMapper.Map<VirtualPath, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Delete)]
    public virtual async Task DeleteAsync(DeleteManyDto input)
    {
        await VirtualPathManager.DeleteManyAsync(input.Ids);
    }

    protected virtual async Task UpdateByInputAsync(VirtualPath entity, VirtualPathCreateOrUpdateDto input)
    {
        await CheckResourceExistsAsync(input.ResourceId);
        entity.SetIsAccessible(input.IsAccessible);
        entity.SetDeadline(input.StartTime, input.EndTime);
    }

    protected virtual async Task CheckResourceExistsAsync(Guid resourceId)
    {
        await ResourceManager.GetAsync(resourceId);
    }


    protected virtual async Task<FileInfoBase> GetAndValidateFileAssociationAsync(Resource entity, string hash)
    {
        // 校验文件夹关联
        if (entity == null)
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
        if (!await ResourceManager.FileExistsAsync(entity.Code, file.Id))
        {
            throw new EntityNotFoundBusinessException(L["File"], hash);
        }

        // 返回文件实例
        return file;
    }
}