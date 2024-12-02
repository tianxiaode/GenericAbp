using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Generic.Abp.FileManagement.ExternalShares.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.FileManagement.ExternalShares;

public interface IExternalShareAppService : IApplicationService
{
    Task<ExternalShareDto> GetMyExternalShareAsync(Guid id);

    Task<PagedResultDto<ExternalShareDto>> GetMyExternalSharesAsync(
        ExternalShareGetListInput input);

    Task DeleteMyExternalSharesAsync(List<Guid> ids);
    Task<ExternalShareDto> GetAsync(Guid id);
    Task<PagedResultDto<ExternalShareDto>> GetListAsync(ExternalShareGetListInput input);
    Task<ExternalShareDto> CreateAsync(ExternalShareCreateDto input);
    Task DeleteManyAsync(List<Guid> ids);
}