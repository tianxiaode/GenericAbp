using System;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class ResourcePermissionDto : EntityDto<Guid>
{
    public virtual Guid? TenantId { get; set; }
    public virtual Guid ResourceId { get; set; }
    public virtual string ProviderName { get; set; }
    public virtual string? ProviderKey { get; set; }
    public virtual int Permissions { get; set; }

    public ResourcePermissionDto(Guid resourceId, string providerName, string? providerKey, int permissions,
        Guid? tenantId)
    {
        ResourceId = resourceId;
        ProviderName = providerName;
        ProviderKey = providerKey;
        Permissions = permissions;
        TenantId = tenantId;
    }
}