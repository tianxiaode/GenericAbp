﻿using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.TailWindCss.Account.Web.MultiTenancy;

public class TenantAppService : ApplicationService, IAbpTenantAppService
{
    protected ITenantStore TenantStore { get; }

    public TenantAppService(ITenantStore tenantStore)
    {
        TenantStore = tenantStore;
    }

    public virtual async Task<FindTenantResultDto> FindTenantByNameAsync(string name)
    {
        var tenant = await TenantStore.FindAsync(name);

        if (tenant == null)
        {
            return new FindTenantResultDto { Success = false };
        }

        return new FindTenantResultDto
        {
            Success = true,
            TenantId = tenant.Id,
            Name = tenant.Name,
            IsActive = tenant.IsActive
        };
    }

    public virtual async Task<FindTenantResultDto> FindTenantByIdAsync(Guid id)
    {
        var tenant = await TenantStore.FindAsync(id);

        if (tenant == null)
        {
            return new FindTenantResultDto { Success = false };
        }

        return new FindTenantResultDto
        {
            Success = true,
            TenantId = tenant.Id,
            Name = tenant.Name,
            IsActive = tenant.IsActive
        };
    }
}