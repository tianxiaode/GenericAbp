using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using Generic.Abp.AuditLogging.AuditLogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.AuditLogging.AuditLogs;

[RemoteService(Name = AuditLoggingRemoteServiceConsts.RemoteServiceName)]
[Area(AuditLoggingRemoteServiceConsts.ModuleName)]
[ControllerName("AuditLogs")]
[Route("api/audit-logs")]
public class AuditLogController : AuditLoggingController, IAuditLogAppService
{
    public AuditLogController(IAuditLogAppService appService)
    {
        AppService = appService;
    }

    protected IAuditLogAppService AppService { get; }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<AuditLogDto> GetAsync(Guid id)
    {
        return AppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<AuditLogDto>> GetListAsync(AuditLogGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpGet]
    [Route(("/api/average-execution-duration-per-day"))]
    public Task<Dictionary<DateTime, double>> GetAverageExecutionDurationPerDayAsync(
        GetAverageExecutionDurationPerDayInput input)
    {
        return AppService.GetAverageExecutionDurationPerDayAsync(input);
    }

    [HttpGet]
    [Route(("/api/entity-changes"))]
    public Task<PagedResultDto<EntityChangeDto>> GetEntityChangesAsync(GetEntityChangesListInput input)
    {
        return AppService.GetEntityChangesAsync(input);
    }

    [HttpGet]
    [Route(("application-names"))]
    public Task<ListResultDto<string>> GetAllApplicationNamesAsync(string? filter)
    {
        return AppService.GetAllApplicationNamesAsync(filter);
    }

    [HttpGet]
    [Route(("urls"))]
    public Task<ListResultDto<string>> GetAllUrlsAsync(string? filter)
    {
        return AppService.GetAllUserNamesAsync(filter);
    }

    [HttpGet]
    [Route(("user-names"))]
    public Task<ListResultDto<string>> GetAllUserNamesAsync(string? filter)
    {
        return AppService.GetAllClientIdsAsync(filter);
    }

    [HttpGet]
    [Route(("client-ids"))]
    public Task<ListResultDto<string>> GetAllClientIdsAsync(string? filter)
    {
        return AppService.GetAllClientIdsAsync(filter);
    }

    [HttpGet]
    [Route(("correlation-ids"))]
    public Task<ListResultDto<string>> GetAllCorrelationIdsAsync(string? filter)
    {
        return AppService.GetAllCorrelationIdsAsync(filter);
    }
}