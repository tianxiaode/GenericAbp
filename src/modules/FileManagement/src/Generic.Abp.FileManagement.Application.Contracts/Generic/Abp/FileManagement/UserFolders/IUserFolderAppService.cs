using System;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.UserFolders.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.FileManagement.UserFolders;

public interface IUserFolderAppService : IApplicationService
{
    Task<UserFolderDto> GetAsync(Guid id);
    Task<PagedResultDto<UserFolderDto>> GetListAsync(UserFolderGetListInput input);
    Task<UserFolderDto> CreateAsync(UserFolderCreateDto input);
    Task<UserFolderDto> UpdateAsync(Guid id, UserFolderUpdateDto input);
    Task DeleteAsync(Guid id);
}