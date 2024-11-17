﻿using System;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.VirtualPaths;

public class VirtualPathPermission : AuditedAggregateRoot<Guid>, IMultiTenant, IPermission
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid TargetId { get; protected set; }

    // 'R' for Role, 'U' for User,"A" for Authenticated User, "E" for Everyone
    [DisplayName("Permission:ProviderName")]
    public virtual string ProviderName { get; protected set; } = "A";

    // User ID or Role Name, empty for authenticated user
    [DisplayName("Permission:ProviderKey")]
    public virtual string? ProviderKey { get; protected set; }

    [DisplayName("Permission:CanRead")] public virtual bool CanRead { get; protected set; }
    [DisplayName("Permission:CanWrite")] public virtual bool CanWrite { get; protected set; }
    [DisplayName("Permission:CanDelete")] public virtual bool CanDelete { get; protected set; }

    public VirtualPathPermission(Guid id, Guid targetId, string providerName,
        string? providerKey = null,
        Guid? tenantId = null) :
        base(id)
    {
        Check.NotNullOrWhiteSpace(providerName, nameof(providerName));

        TargetId = targetId;
        ProviderName = providerName;
        ProviderKey = providerKey;
        TenantId = tenantId;
    }


    public void SetPermissions(bool canRead, bool canWrite, bool canDelete)
    {
        CanRead = canRead;
        CanWrite = canWrite;
        CanDelete = canDelete;
    }
}