using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.FileInfoBases;
using System;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Resources;

public class Resource : TreeAuditedAggregateRoot<Resource>
{
    public virtual ResourceType Type { get; protected set; }
    public virtual FileInfoBase? FileInfoBase { get; protected set; }
    public virtual Guid? FileInfoBaseId { get; protected set; }
    public virtual bool HasConfiguration { get; protected set; }
    public virtual bool HasPermissions { get; protected set; }
    public virtual ICollection<ResourcePermission> Permissions { get; set; } = default!;
    public virtual Guid? OwnerId { get; protected set; }
    public virtual bool IsStatic { get; protected set; }
    public virtual bool IsAccessible { get; protected set; }
    public virtual string? FileExtension { get; protected set; }
    public virtual long? FileSize { get; protected set; }

    public Resource(Guid id, string name, ResourceType type, bool isStatic = false, Guid? ownerId = null,
        Guid? tenantId = null) : base(id,
        name, tenantId)
    {
        Type = type;
        IsStatic = isStatic;
        OwnerId = ownerId;
        HasConfiguration = false;
        HasPermissions = false;
        IsAccessible = true;
    }

    public virtual void SetFileInfoBase(Guid? fileInfoBaseId)
    {
        FileInfoBaseId = fileInfoBaseId;
    }

    public virtual void SetIsAccessible(bool isAccessible)
    {
        IsAccessible = isAccessible;
    }

    public virtual void SetHasConfiguration(bool hasConfiguration)
    {
        HasConfiguration = hasConfiguration;
    }

    public virtual void SetHasPermissions(bool hasPermissions)
    {
        HasPermissions = hasPermissions;
    }

    public virtual void SetOwner(Guid ownerId)
    {
        OwnerId = ownerId;
    }

    public virtual void SetFileExtension(string fileExtension)
    {
        FileExtension = fileExtension.ToLower();
    }

    public virtual void SetFileSize(long fileSize)
    {
        FileSize = fileSize;
    }
}