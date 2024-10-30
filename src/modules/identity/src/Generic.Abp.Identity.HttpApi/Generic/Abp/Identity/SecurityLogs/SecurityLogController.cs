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
    public Task<ListResultDto<string>> GetAllIdentitiesAsync()
    {
        return Service.GetAllIdentitiesAsync();
    }


    [HttpGet]
    [Route("actions")]
    public Task<ListResultDto<string>> GetAllActionsAsync()
    {
        return Service.GetAllActionsAsync();
    }

    [HttpGet]
    [Route("user-names")]
    public Task<ListResultDto<string>> GetAllUserNamesAsync()
    {
        return Service.GetAllUserNamesAsync();
    }

    [HttpGet]
    [Route("client-ids")]
    public Task<ListResultDto<string>> GetAllClientIdsAsync()
    {
        return Service.GetAllClientIdsAsync();
    }

    [HttpGet]
    [Route("correlation-ids")]
    public Task<ListResultDto<string>> GetAllCorrelationIdsAsync()
    {
        return Service.GetAllCorrelationIdsAsync();
    }
}