using Generic.Abp.Extensions.RemoteContents;
using Generic.Abp.FileManagement.Dtos;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using System;
using System.Threading.Tasks;
using Generic.Abp.Extensions.Entities.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathAppService : IApplicationService
{
    Task<IRemoteContent> GetFileAsync(string path, string hash, GetFileDto input);
    Task<VirtualPathDto> GetAsync(Guid id);
    Task<VirtualPathDto> FindByNameAsync(string name);
    Task<PagedResultDto<VirtualPathDto>> GetListAsync(VirtualPathGetListInput input);
    Task<VirtualPathDto> CreateAsync(VirtualPathCreateDto input);
    Task<VirtualPathDto> UpdateAsync(Guid id, VirtualPathUpdateDto input);
    Task DeleteAsync(DeleteManyDto input);
}