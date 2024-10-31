using Generic.Abp.AuditLogging.AuditLogs.Dtos;
using Generic.Abp.AuditLogging.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;

namespace Generic.Abp.AuditLogging.AuditLogs;

[RemoteService(false)]
[Authorize(AuditLoggingPermissions.AuditLogs)]
[DisableAuditing]
public class AuditLogAppService : AuditLoggingAppService, IAuditLogAppService
{
    public AuditLogAppService(IAuditLogExtensionRepository repository)
    {
        Repository = repository;
    }

    protected IAuditLogExtensionRepository Repository { get; }


    public virtual async Task<AuditLogDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<AuditLog, AuditLogDto>(await Repository.GetAsync(id));
    }

    public virtual async Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetListInput input)
    {
        var predicate = await Repository.BuildPredicateAsync(input.Filter, input.StartTime, input.EndTime,
            input.ApplicationName, input.UserName, input.Url, input.ClientId, input.CorrelationId,
            input.MinExecutionDuration, input.MaxExecutionDuration);
        var totalCount = await Repository.GetCountAsync(predicate);
        if (totalCount == 0)
        {
            return new PagedResultDto<AuditLogDto>(0, new List<AuditLogDto>());
        }

        var securityLogs =
            await Repository.GetListAsync(predicate, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<AuditLogDto>(totalCount,
            ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(securityLogs));
    }

    public virtual async Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        GetAverageExecutionDurationPerDayInput input)
    {
        return await Repository.GetAverageExecutionDurationPerDayAsync(input.StartTime, input.EndTime);
    }

    public virtual async Task<PagedResultDto<EntityChangeDto>> GetEntityChangesAsync(GetEntityChangesListInput input)
    {
        var predicate = await Repository.BuildPredicateAsync(input.Filter, null, input.StartTime, input.EndTime,
            input.ChangeType, null);
        var totalCount = await Repository.GetEntityChangesCoutAsync(predicate);
        if (totalCount == 0)
        {
            return new PagedResultDto<EntityChangeDto>(0, new List<EntityChangeDto>());
        }

        var entityChanges =
            await Repository.GetEntityChangesAsync(predicate, input.Sorting, input.MaxResultCount, input.SkipCount);
        return new PagedResultDto<EntityChangeDto>(totalCount,
            ObjectMapper.Map<List<EntityChange>, List<EntityChangeDto>>(entityChanges));
    }

    public virtual async Task<ListResultDto<string>> GetAllApplicationNamesAsync(string? filter)
    {
        return new ListResultDto<string>((await Repository.GetAllApplicationNamesAsync())
            .Where(m => !string.IsNullOrEmpty(m))
            .WhereIf(!string.IsNullOrEmpty(filter), m => m.Contains(filter!, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public virtual async Task<ListResultDto<string>> GetAllUrlsAsync(string? filter)
    {
        return new ListResultDto<string>((await Repository.GetAllUrlsAsync())
            .Where(m => !string.IsNullOrEmpty(m))
            .WhereIf(!string.IsNullOrEmpty(filter), m => m.Contains(filter!, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public virtual async Task<ListResultDto<string>> GetAllUserNamesAsync(string? filter)
    {
        return new ListResultDto<string>((await Repository.GetAllUserNamesAsync())
            .Where(m => !string.IsNullOrEmpty(m))
            .WhereIf(!string.IsNullOrEmpty(filter), m => m.Contains(filter!, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public virtual async Task<ListResultDto<string>> GetAllClientIdsAsync(string? filter)
    {
        return new ListResultDto<string>((await Repository.GetAllClientIdsAsync())
            .Where(m => !string.IsNullOrEmpty(m))
            .WhereIf(!string.IsNullOrEmpty(filter), m => m.Contains(filter!, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }

    public virtual async Task<ListResultDto<string>> GetAllCorrelationIdsAsync(string? filter)
    {
        return new ListResultDto<string>((await Repository.GetAllCorrelationIdsAsync())
            .Where(m => !string.IsNullOrEmpty(m))
            .WhereIf(!string.IsNullOrEmpty(filter), m => m.Contains(filter!, StringComparison.OrdinalIgnoreCase))
            .ToList());
    }
}