﻿using Asp.Versioning;
using Generic.Abp.Extensions.Entities.Dtos;
using Generic.Abp.FileManagement.ExternalShares.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.ExternalShares;

[RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
[Area(FileManagementRemoteServiceConsts.RemoteServiceName)]
[ControllerName("ExternalShares")]
[Route("api/external-shares")]
public class ExternalShareController(IExternalShareAppService appService)
    : FileManagementController, IExternalShareAppService
{
    protected IExternalShareAppService AppService { get; } = appService;

    [HttpGet]
    [Route("my/{id:guid}")]
    public Task<ExternalShareDto> GetMyExternalShareAsync(Guid id)
    {
        return AppService.GetMyExternalShareAsync(id);
    }

    [HttpGet]
    [Route("my")]
    public Task<PagedResultDto<ExternalShareDto>> GetMyExternalSharesAsync(ExternalShareGetListInput input)
    {
        return AppService.GetMyExternalSharesAsync(input);
    }

    [HttpDelete]
    [Route("my")]
    public Task DeleteMyExternalSharesAsync([FromBody] DeleteManyDto input)
    {
        return AppService.DeleteMyExternalSharesAsync(input);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public Task<ExternalShareDto> GetAsync(Guid id)
    {
        return AppService.GetAsync(id);
    }

    [HttpGet]
    public Task<PagedResultDto<ExternalShareDto>> GetListAsync(ExternalShareGetListInput input)
    {
        return AppService.GetListAsync(input);
    }

    [HttpPost]
    public Task<ExternalShareDto> CreateAsync([FromBody] ExternalShareCreateDto input)
    {
        return AppService.CreateAsync(input);
    }

    [HttpDelete]
    public Task DeleteManyAsync([FromBody] DeleteManyDto input)
    {
        return AppService.DeleteManyAsync(input);
    }
}