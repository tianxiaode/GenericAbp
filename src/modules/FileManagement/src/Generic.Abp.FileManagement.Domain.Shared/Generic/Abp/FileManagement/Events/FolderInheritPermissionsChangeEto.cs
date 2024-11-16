using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Events;

[Serializable]
public class FolderInheritPermissionsChangeEto(Guid id, Guid? tenantId, bool inheritPermissions) : IMultiTenant
{
    public Guid Id { get; set; } = id;

    public Guid? TenantId { get; set; } = tenantId;

    public bool InheritPermissions { get; set; } = inheritPermissions;
}