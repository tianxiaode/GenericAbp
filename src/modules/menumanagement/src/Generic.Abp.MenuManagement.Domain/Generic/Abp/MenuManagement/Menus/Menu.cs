using Generic.Abp.Extensions.Trees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.MenuManagement.Menus;

[Serializable]
public class Menu : AuditedAggregateRoot<Guid>, ITree<Menu>, IMultiTenant, IHasEntityVersion
{
    public virtual Menu? Parent { get; set; } = default!;
    public virtual ICollection<Menu>? Children { get; set; } = default!;
    public virtual string Code { get; protected set; } = default!;
    public virtual Guid? ParentId { get; protected set; }
    public virtual Guid? TenantId { get; protected set; }
    public virtual int EntityVersion { get; protected set; }
    [Display(Name = "Menu:Name")] public virtual string Name { get; protected set; } = default!;
    [Display(Name = "Menu:Icon")] public virtual string? Icon { get; protected set; } = default!;
    [Display(Name = "Menu:IsEnabled")] public virtual bool IsEnabled { get; protected set; }
    [Display(Name = "Menu:IsStatic")] public virtual bool IsStatic { get; protected set; }
    [Display(Name = "Menu:Order")] public virtual int Order { get; protected set; }
    [Display(Name = "Menu:Router")] public virtual string? Router { get; protected set; } = default!;

    public Menu(Guid id, Guid? parentId, string name, Guid? tenantId = null,
        bool isStatic = false) : base(id)
    {
        Check.NotNull(name, nameof(Name));

        ParentId = parentId;
        Name = name;
        IsEnabled = true;
        Order = 1;
        TenantId = tenantId;
        EntityVersion = 0;
        IsStatic = isStatic;
    }

    public virtual void Rename(string name)
    {
        Check.NotNull(name, nameof(Name));
        Name = name;
    }

    public virtual void SetIcon(string? icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            return;
        }

        Icon = icon;
    }

    public virtual void SetRouter(string? router)
    {
        if (string.IsNullOrEmpty(router))
        {
            return;
        }

        Router = router;
    }

    public virtual void SetOrder(int order)
    {
        Order = order;
    }

    public virtual void Disable()
    {
        IsEnabled = false;
    }

    public virtual void Enable()
    {
        IsEnabled = true;
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