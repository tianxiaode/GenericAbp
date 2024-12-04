using Asp.Versioning;
using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.VirtualPaths;

[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[ControllerName("VirtualPaths")]
[Route("api/virtual-paths")]
public class VirtualPathController(IVirtualPathAppService appService) : FileManagementController, IVirtualPathAppService
{
    protected IVirtualPathAppService AppService { get; } = appService;

    [HttpGet]
    [Route("files/{path}/{hash}")]
    public Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input)
    {
        return AppService.GetFileAsync(path, hash, input);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<VirtualPathDto> GetAsync(Guid id)
    {
        return AppService.GetAsync(id);
    }

    [HttpGet]
    [Route("find-by-name/{name}")]
    public Task<VirtualPathDto> FindByNameAsync(string name)
    {
        return AppService.FindByNameAsync(name);
    }

    [HttpGet]
    public Task<PagedResultDto<VirtualPathDto>> GetListAsync(IVirtualPathGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<VirtualPathDto> CreateAsync([FromBody] VirtualPathCreateDto input)
    {
        return AppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public Task<VirtualPathDto> UpdateAsync(Guid id, [FromBody] VirtualPathUpdateDto input)
    {
        return AppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public Task DeleteAsync(Guid id)
    {
        return AppService.DeleteAsync(id);
    }
}