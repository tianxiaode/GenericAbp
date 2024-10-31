using System.Threading.Tasks;
using System;
using Generic.Abp.AuditLogging.AuditLogs.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace Generic.Abp.AuditLogging.AuditLogs;

public interface IAuditLogAppService : IApplicationService
{
    Task<AuditLogDto> GetAsync(Guid id);
    Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetListInput input);

    Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        GetAverageExecutionDurationPerDayInput input);

    Task<PagedResultDto<EntityChangeDto>> GetEntityChangesAsync(GetEntityChangesListInput input);
    Task<ListResultDto<string>> GetAllApplicationNamesAsync(string? filter);
    Task<ListResultDto<string>> GetAllUrlsAsync(string? filter);
    Task<ListResultDto<string>> GetAllUserNamesAsync(string? filter);
    Task<ListResultDto<string>> GetAllClientIdsAsync(string? filter);
    Task<ListResultDto<string>> GetAllCorrelationIdsAsync(string? filter);
}