using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Permissions;
using Generic.Abp.FileManagement.Resources;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;


namespace Generic.Abp.FileManagement.VirtualPaths;

[RemoteService(false)]
public class VirtualPathAppService(
    VirtualPathManager virtualPathManager,
    IResourceRepository repository,
    FileInfoBaseManager fileInfoBaseManager
)
    : FileManagementAppService, IVirtualPathAppService
{
    protected VirtualPathManager VirtualPathManager { get; } = virtualPathManager;
    protected IResourceRepository Repository { get; } = repository;
    protected FileInfoBaseManager FileInfoBaseManager { get; } = fileInfoBaseManager;

    [AllowAnonymous]
    public virtual async Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input)
    {
        throw new NotImplementedException();
        var entity = await VirtualPathManager.FindByPathAsync(path);
        //是否有权限
        // var canRead = await VirtualPathManager.CadReadAsync(entity, CurrentUser.Id);
        // if (!canRead)
        // {
        //     throw new AbpAuthorizationException(L["AccessDenied"]);
        // }
        //
        // //是否存在
        // var file = await FileInfoBaseManager.FindByHashAsync(hash);
        // if (file == null)
        // {
        //     throw new EntityNotFoundBusinessException(L["File"], hash);
        // }
        //
        // var exits = await FileManager.FileExistsAsync(entity.FolderId, file.Id);
        // if (!exits)
        // {
        //     throw new EntityNotFoundBusinessException(L["File"], hash);
        // }
        //
        //return await FileInfoBaseManager.GetFileAsync(file, input.ChunkSize, input.Index);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> GetAsync(Guid id)
    {
        var entity = await VirtualPathManager.GetAsync(id);
        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<VirtualPathDto> FindByNameAsync(string name)
    {
        var entity = await VirtualPathManager.FindByPathAsync(name);
        return ObjectMapper.Map<Resource, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Default)]
    public virtual async Task<PagedResultDto<VirtualPathDto>> GetListAsync(VirtualPathGetListInput input)
    {
        throw new NotImplementedException();
        // var predicate = await Repository.BuildPredicateAsync(input.Filter);
        // var count = await Repository.GetCountAsync(predicate);
        // var list = await Repository.GetListAsync(predicate, input.Sorting, input.MaxResultCount, input.SkipCount);
        // var result =
        //     new PagedResultDto<VirtualPathDto>(count, ObjectMapper.Map<List<VirtualPath>, List<VirtualPathDto>>(list));
        // return result;
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Create)]
    public virtual async Task<VirtualPathDto> CreateAsync(VirtualPathCreateDto input)
    {
        throw new NotImplementedException();
        // var entity = new VirtualPath(GuidGenerator.Create(), input.FolderId, input.Path, CurrentTenant.Id);
        // await VirtualPathManager.CreateAsync(entity);
        // return ObjectMapper.Map<VirtualPath, VirtualPathDto>(entity);
    }

    [Authorize(FileManagementPermissions.VirtualPaths.Update)]
    public virtual async Task<VirtualPathDto> UpdateAsync(Guid id, VirtualPathUpdateDto input)
    {
        throw new NotImplementedException();
        // var entity = await VirtualPathManager.GetAsync(id);
        // entity.ChangeVirtualPath(input.Path);
        // await VirtualPathManager.UpdateAsync(entity);
        // return ObjectMapper.Map<VirtualPath, VirtualPathDto>(entity);
    }

    //[Authorize(FileManagementPermissions.VirtualPaths.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await VirtualPathManager.GetAsync(id);
        await VirtualPathManager.DeleteAsync(entity);
    }

    #region Permission

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task<ListResultDto<VirtualPathPermissionDto>> GetPermissionsAsync(Guid id)
    {
        throw new NotImplementedException();
        // var list = await VirtualPathManager.GetPermissionsAsync(id);
        // return new ListResultDto<VirtualPathPermissionDto>(
        //     ObjectMapper.Map<List<VirtualPathPermission>, List<VirtualPathPermissionDto>>(list));
    }

    [Authorize(FileManagementPermissions.VirtualPaths.ManagePermissions)]
    public virtual async Task UpdatePermissionAsync(Guid id,
        VirtualPathPermissionCreateOrUpdateDto input)
    {
        throw new NotImplementedException();
        // var entity = await VirtualPathManager.GetAsync(id);
        // var permissions = input.Permissions.Select(m =>
        //     new VirtualPathPermission(m.Id ?? GuidGenerator.Create(), id, m.ProviderName, m.ProviderKey,
        //         m.CanRead, m.CanWrite, m.CanDelete, CurrentTenant.Id)).ToList();
        // await VirtualPathManager.SetPermissionsAsync(entity, permissions);
    }

    #endregion
}