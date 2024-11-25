using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.FileInfoBases;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generic.Abp.FileManagement.Resources;

public class Resource : TreeAuditedAggregateRoot<Resource>
{
    public virtual ResourceType Type { get; protected set; }
    [NotMapped] public virtual IFileInfoBase? FileInfoBase { get; protected set; }
    public virtual Guid? FileInfoBaseId { get; protected set; }
    public virtual Guid? FolderId { get; protected set; }
    public virtual bool IsStatic { get; protected set; }

    public Resource(Guid id, string name, ResourceType type, bool isStatic = false, Guid? tenantId = null) : base(id,
        name, tenantId)
    {
        Type = type;
        IsStatic = isStatic;
    }

    public void SetFileInfoBase(IFileInfoBase? fileInfoBase)
    {
        FileInfoBaseId = fileInfoBase?.Id;
    }

    public void SetFolderId(Guid? folderId)
    {
        FolderId = folderId;
    }
}