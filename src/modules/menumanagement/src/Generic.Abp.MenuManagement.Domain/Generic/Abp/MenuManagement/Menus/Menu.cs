using Generic.Abp.Extensions.Trees;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace Generic.Abp.MenuManagement.Menus;

[Serializable]
public class Menu : TreeAuditedAggregateRoot<Menu>, IHasEntityVersion
{
    public virtual int EntityVersion { get; protected set; }
    [Display(Name = "Menu:Icon")] public virtual string? Icon { get; protected set; }
    [Display(Name = "Menu:IsEnabled")] public virtual bool IsEnabled { get; protected set; }
    [Display(Name = "Menu:IsStatic")] public virtual bool IsStatic { get; protected set; }
    [Display(Name = "Menu:Order")] public virtual int Order { get; protected set; }
    [Display(Name = "Menu:Router")] public virtual string? Router { get; protected set; }

    public Menu(Guid id, Guid? parentId, string name, Guid? tenantId = null,
        bool isStatic = false) : base(id, name, tenantId)
    {
        Check.NotNull(name, nameof(Name));

        ParentId = parentId;
        IsEnabled = true;
        Order = 1;
        EntityVersion = 0;
        IsStatic = isStatic;
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
}