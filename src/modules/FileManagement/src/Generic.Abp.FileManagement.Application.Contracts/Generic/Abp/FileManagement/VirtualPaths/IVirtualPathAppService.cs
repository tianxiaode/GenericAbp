using System.Threading.Tasks;
using System;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathAppService : IApplicationService
{
    Task<VirtualPathDto> GetAsync(Guid id);
    Task<VirtualPathDto> FindByNameAsync(string name);
    Task<PagedResultDto<ResourceBaseDto>> GetListAsync(VirtualPathGetListInput input);
    Task<ResourceBaseDto> CreateAsync(VirtualPathCreateDto input);
    Task<ResourceBaseDto> UpdateAsync(Guid id, VirtualPathUpdateDto input);
    Task DeleteAsync(Guid id);

    Task<ListResultDto<ResourcePermissionDto>> GetPermissionsAsync(Guid id);

    Task UpdatePermissionAsync(Guid id,
        ResourcePermissionsCreateOrUpdateDto input);
}