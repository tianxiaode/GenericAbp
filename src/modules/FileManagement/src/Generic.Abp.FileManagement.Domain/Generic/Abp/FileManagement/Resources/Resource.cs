using Generic.Abp.Extensions.Trees;
using Generic.Abp.FileManagement.FileInfoBases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Generic.Abp.FileManagement.Resources;

public class Resource : AuditedAggregateRoot<Guid>, ITree<Resource>, IResource
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Name { get; protected set; }
    public virtual string Code { get; protected set; } = default!;
    public Guid? ParentId { get; protected set; }
    public virtual ResourceType Type { get; protected set; }

    [NotMapped] public virtual IFileInfoBase? FileInfoBase { get; protected set; }
    public virtual Guid? FileInfoBaseId { get; protected set; }
    public virtual Guid? FolderId { get; protected set; }
    public virtual bool IsStatic { get; protected set; }

    [NotMapped] public virtual Resource? Parent { get; set; }

    [NotMapped] public virtual ICollection<Resource>? Children { get; set; }

    public Resource(Guid id, string name, ResourceType type, bool isStatic = false, Guid? tenantId = null) : base(id)
    {
        Name = name;
        Type = type;
        IsStatic = isStatic;
        TenantId = tenantId;
    }

    public void SetFileInfoBase(IFileInfoBase fileInfoBase)
    {
        FileInfoBaseId = fileInfoBase.Id;
    }

    public void SetFolderId(Guid? folderId)
    {
        FolderId = folderId;
    }

    public void MoveTo(Guid? parentId)
    {
        ParentId = parentId;
    }

    public void SetCode(string code)
    {
        Code = code;
    }
}