using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Generic.Abp.FileManagement.Resources.Dtos;
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
    public Task<ResourceBaseDto> GetAsync(Guid id)
    {
        return AppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<ResourceBaseDto>> GetListAsync(UserFolderGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("users")]
    public Task<PagedResultDto<UserDto>> GetUsersAsync(UserGetListInput input)
    {
        return AppService.GetUsersAsync(input);
    }

    [HttpPost]
    public Task<ResourceBaseDto> CreateAsync([FromBody] UserFolderCreateDto input)
    {
        return AppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<ResourceBaseDto> UpdateAsync(Guid id, [FromBody] UserFolderUpdateDto input)
    {
        return AppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return AppService.DeleteAsync(id);
    }
}