using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Resources.Dtos;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.FileManagement.UserFolders;

public interface IUserFolderAppService : IApplicationService
{
    Task<ResourceBaseDto> GetAsync(Guid id);
    Task<PagedResultDto<ResourceBaseDto>> GetListAsync(UserFolderGetListInput input);
    Task<PagedResultDto<UserDto>> GetUsersAsync(UserGetListInput input);
    Task<ResourceBaseDto> CreateAsync(UserFolderCreateDto input);
    Task<ResourceBaseDto> UpdateAsync(Guid id, UserFolderUpdateDto input);
    Task DeleteAsync(Guid id);
}