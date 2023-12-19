using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.Domain.Entities;
using Generic.Abp.Domain.Entities.Auditing;
using Generic.Abp.Domain.Trees;
using Volo.Abp.Validation;

namespace Generic.Abp.MenuManagement.Menus;

[Serializable]
public class Menu : GenericAuditedAggregateRootWithTranslation, ITree<Menu>
{
    public Menu(Guid id) : base(id)
    {
    }

    public Menu Parent { get; set; }
    public ICollection<Menu> Children { get; set; }
    public string Code { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }

    [Display(Name = "Menu:DisplayName")] public string DisplayName { get; set; } = string.Empty;

    [Display(Name = "Menu:Icon")] public string Icon { get; set; }

    [Display(Name = "Menu:IsSelectable")] public bool IsSelectable { get; set; } = true;
    [Display(Name = "Menu:IsDisabled")] public bool IsDisabled { get; set; } = false;

    [Display(Name = "Menu:Order")] public int Order { get; set; } = 0;

    [Display(Name = "Menu:Router")] public string Router { get; set; }

    [Display(Name = "Menu:GroupName")] public string GroupName { get; set; }

    public virtual void SetIcon(string icon)
    {
        Icon = icon;
    }

    public virtual void SetRouter(string router)
    {
        Router = router;
    }

    public virtual void SetGroupName(string groupName)
    {
        GroupName = groupName;
    }

    public virtual void SetOrder(int order)
    {
        Order = order;
    }
}