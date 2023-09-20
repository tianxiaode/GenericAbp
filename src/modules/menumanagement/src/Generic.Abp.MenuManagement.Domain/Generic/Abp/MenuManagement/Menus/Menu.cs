using System;
using System.Collections.Generic;
using Generic.Abp.Domain.Entities.Auditing;
using Generic.Abp.Domain.Trees;

namespace Generic.Abp.MenuManagement.Menus;

[Serializable]
public class Menu : GenericAuditedAggregateRootWithTranslation, ITree<Menu>
{
    public Menu Parent { get; set; }
    public ICollection<Menu> Children { get; set; }
    public string Code { get; set; }
    public Guid? ParentId { get; set; }
    public string DisplayName { get; set; }
    public string Icon { get; set; }
    public bool IsSelectable { get; set; }
    public bool IsDisabled { get; set; }
    public int Order { get; set; }
    public string Router { get; set; }
    public string GroupName { get; set; }

    public Menu(Guid id) : base(id)
    {
        DisplayName = string.Empty;
        IsSelectable = true;
        IsDisabled = false;
        Code = string.Empty;
    }
}