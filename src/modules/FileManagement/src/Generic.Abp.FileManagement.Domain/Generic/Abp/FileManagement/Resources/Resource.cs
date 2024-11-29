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
    public virtual Guid? FolderId { get; protected set; }
    public virtual bool IsStatic { get; protected set; }
    public virtual Resource? Folder { get; protected set; }
    public virtual ResourceConfiguration? Configuration { get; protected set; }
    public virtual Guid? ConfigurationId { get; protected set; }
    public virtual ICollection<ResourcePermission> Permissions { get; set; } = default!;

    public Resource(Guid id, string name, ResourceType type, bool isStatic = false, Guid? tenantId = null) : base(id,
        name, tenantId)
    {
        Type = type;
        IsStatic = isStatic;
    }

    public void SetFileInfoBase(Guid? fileInfoBaseId)
    {
        FileInfoBaseId = fileInfoBaseId;
    }

    public void SetFolderId(Guid? folderId)
    {
        FolderId = folderId;
    }

    public void SetConfiguration(Guid resourceConfigurationId)
    {
        ConfigurationId = resourceConfigurationId;
    }
}