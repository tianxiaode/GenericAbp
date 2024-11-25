using System.Threading.Tasks;
using System;
using Generic.Abp.FileManagement.VirtualPaths.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.VirtualPaths;

public interface IVirtualPathAppService : IApplicationService
{
    Task<VirtualPathDto> GetAsync(Guid id);
    Task<VirtualPathDto> FindByNameAsync(string name);
    Task<PagedResultDto<VirtualPathDto>> GetListAsync(VirtualPathGetListInput input);
    Task<VirtualPathDto> CreateAsync(VirtualPathCreateDto input);
    Task<VirtualPathDto> UpdateAsync(Guid id, VirtualPathUpdateDto input);
    Task DeleteAsync(Guid id);

    Task<ListResultDto<VirtualPathPermissionDto>> GetPermissionsAsync(Guid id);

    Task UpdatePermissionAsync(Guid id,
        VirtualPathPermissionCreateOrUpdateDto input);
}