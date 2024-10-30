using Asp.Versioning;
using Generic.Abp.Identity.SecurityLogs.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.Identity.SecurityLogs;

[RemoteService(Name = "AbpIdentity")]
[Area("identity")]
[ControllerName("SecurityLog")]
[Route("api/security-logs")]
public class SecurityLogController : IdentityController, ISecurityLogAppService
{
    protected ISecurityLogAppService Service { get; }

    public SecurityLogController(ISecurityLogAppService service)
    {
        Service = service;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<SecurityLogDto> GetAsync(Guid id)
    {
        return Service.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<SecurityLogDto>> GetListAsync(SecurityLogGetListInput input)
    {
        return Service.GetListAsync(input);
    }

    [HttpGet]
    [Route("application-names")]
    public Task<ListResultDto<string>> GetAllApplicationNamesAsync(string? filter)
    {
        return Service.GetAllApplicationNamesAsync(filter);
    }

    [HttpGet]
    [Route("identities")]
    public Task<ListResultDto<string>> GetAllIdentitiesAsync(string? filter)
    {
        return Service.GetAllIdentitiesAsync(filter);
    }


    [HttpGet]
    [Route("actions")]
    public Task<ListResultDto<string>> GetAllActionsAsync(string? filter)
    {
        return Service.GetAllActionsAsync(filter);
    }

    [HttpGet]
    [Route("user-names")]
    public Task<ListResultDto<string>> GetAllUserNamesAsync(string? filter)
    {
        return Service.GetAllUserNamesAsync(filter);
    }

    [HttpGet]
    [Route("client-ids")]
    public Task<ListResultDto<string>> GetAllClientIdsAsync(string? filter)
    {
        return Service.GetAllClientIdsAsync(filter);
    }

    [HttpGet]
    [Route("correlation-ids")]
    public Task<ListResultDto<string>> GetAllCorrelationIdsAsync(string? filter)
    {
        return Service.GetAllCorrelationIdsAsync(filter);
    }
}