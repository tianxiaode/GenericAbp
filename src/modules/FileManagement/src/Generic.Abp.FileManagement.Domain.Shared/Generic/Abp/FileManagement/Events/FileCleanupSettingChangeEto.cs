using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class FileCleanupSettingChangeEto(Guid? tenantId = null) : IMultiTenant
{
    public Guid? TenantId { get; set; } = tenantId;
}