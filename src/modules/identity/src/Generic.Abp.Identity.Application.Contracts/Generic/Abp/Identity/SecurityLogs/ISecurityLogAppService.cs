using System;
using System.Threading.Tasks;
using Generic.Abp.Identity.SecurityLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Generic.Abp.Identity.SecurityLogs;

public interface ISecurityLogAppService : IApplicationService
{
    Task<SecurityLogDto> GetAsync(Guid id);
    Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetListInput input);
    Task<ListResultDto<string>> GetAllApplicationNamesAsync(string? filter);
    Task<ListResultDto<string>> GetAllIdentitiesAsync();
    Task<ListResultDto<string>> GetAllActionsAsync();
    Task<ListResultDto<string>> GetAllUserNamesAsync();
    Task<ListResultDto<string>> GetAllClientIdsAsync();
    Task<ListResultDto<string>> GetAllCorrelationIdsAsync();
}