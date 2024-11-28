using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.FileManagement.Folders;

public interface IPersonalFolderAppService : IApplicationService
{
    Task<ListResultDto<FolderDto>> GetRootFoldersAsync();
    Task<FolderDto> GetAsync(Guid id);
    Task<ListResultDto<FolderDto>> GetListAsync(FolderGetListInput input);
    Task<PagedResultDto<FileDto>> GetFilesAsync(FileGetListInput input);
}