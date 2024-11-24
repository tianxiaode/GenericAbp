using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Jobs;

public class JobBaseArgs : IMultiTenant
{
    public Guid? TenantId { get; set; }
    public string Name { get; set; }

    public JobBaseArgs(Guid? tenantId, string name)
    {
        TenantId = tenantId;
        Name = name;
    }
}