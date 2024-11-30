using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.UserFolders;

[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[ControllerName("UserFolders")]
[Route("api/user-folders")]
public class UserFolderController(IUserFolderAppService appService) : FileManagementController, IUserFolderAppService
{
    protected IUserFolderAppService AppService { get; } = appService;

    [HttpGet]
    [Route("{id:guid}")]
    public Task<UserFolderDto> GetAsync(Guid id)
    {
        return AppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<UserFolderDto>> GetListAsync(UserFolderGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<UserFolderDto> CreateAsync([FromBody] UserFolderCreateDto input)
    {
        return AppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<UserFolderDto> UpdateAsync(Guid id, UserFolderUpdateDto input)
    {
        return AppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return AppService.DeleteAsync(id);
    }
}