using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.FileInfoBases;
using Generic.Abp.FileManagement.Resources.Dtos;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.Folders;

//[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
//[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[ControllerName("PersonalFolder")]
[Route("api/personal-folders")]
public class PersonalFolderController(IPersonalFolderAppService appService)
    : FileManagementController, IPersonalFolderAppService
{
    protected IPersonalFolderAppService AppService { get; } = appService;

    [HttpGet]
    [Route("root")]
    public Task<ListResultDto<FolderDto>> GetRootFoldersAsync()
    {
        return AppService.GetRootFoldersAsync();
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<FolderDto> GetAsync(Guid id)
    {
        return AppService.GetAsync(id);
    }

    [HttpGet]
    public Task<ListResultDto<FolderDto>> GetListAsync(FolderGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("files")]
    public Task<PagedResultDto<FileDto>> GetFilesAsync(FileGetListInput input)
    {
        return AppService.GetFilesAsync(input);
    }
}