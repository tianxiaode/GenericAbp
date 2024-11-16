﻿using System;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Files;

public class FilePermission : AuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual Guid FolderId { get; protected set; }

    // 'R' for Role, 'U' for User, "A" for Authenticated User
    [DisplayName("Permission:ProviderType")]
    public virtual string ProviderType { get; protected set; } = "A";

    // User ID or Role Name, empty for authenticated user
    [DisplayName("Permission:ProviderName")]
    public virtual string? ProviderName { get; protected set; }

    [DisplayName("Permission:CanRead")] public virtual bool CanRead { get; protected set; }
    [DisplayName("Permission:CanWrite")] public virtual bool CanWrite { get; protected set; }
    [DisplayName("Permission:CanDelete")] public virtual bool CanDelete { get; protected set; }

    public FilePermission(Guid id, Guid folderId, string providerType, string? providerName = null,
        Guid? tenantId = null) :
        base(id)
    {
        Check.NotNull(providerType, nameof(providerType));
        FolderId = folderId;
        ProviderType = providerType;
        ProviderName = providerName;
        TenantId = tenantId;
    }

    public void SetPermissions(bool canRead, bool canWrite, bool canDelete)
    {
        CanRead = canRead;
        CanWrite = canWrite;
        CanDelete = canDelete;
    }
}