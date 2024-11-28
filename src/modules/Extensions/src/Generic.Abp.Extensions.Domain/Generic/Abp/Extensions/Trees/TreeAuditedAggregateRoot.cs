using System;
using System.Collections.Generic;
using System.ComponentModel;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Generic.Abp.Extensions.Trees;

public class TreeAuditedAggregateRoot<TEntity> : AuditedAggregateRoot<Guid>, ITree<TEntity>
    where TEntity : TreeAuditedAggregateRoot<TEntity>
{
    public virtual Guid? TenantId { get; protected set; }
    public virtual string Code { get; protected set; } = default!;

    [DisplayName("Display:Parent")] public virtual Guid? ParentId { get; protected set; }

    [DisplayName("Display:Name")] public virtual string Name { get; protected set; }

    [DisplayName("Display:Parent")] public virtual TEntity? Parent { get; set; }

    public virtual ICollection<TEntity>? Children { get; set; }

    public TreeAuditedAggregateRoot(Guid id, string name, Guid? tenantId = null) : base(id)
    {
        Check.NotNullOrEmpty(name, nameof(name));

        Name = name;
        TenantId = tenantId;
    }

    public virtual void Rename(string name)
    {
        Check.NotNull(name, nameof(Name));
        Name = name;
    }

    public virtual void MoveTo(Guid? parentId)
    {
        ParentId = parentId;
    }

    public virtual void SetCode(string code)
    {
        Code = code;
    }
}