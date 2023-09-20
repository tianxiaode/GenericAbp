using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuDto : ExtensibleAuditedEntityDto<Guid>
{
    public string Code { get; set; }
    public string Postcode { get; set; }
    public string DisplayName { get; set; }
    public Guid? ParentId { get; set; }
    public bool Leaf { get; set; }
    public MenuDto Parent { get; set; }
    public List<MenuTranslationDto> Translations { get; set; }
    public ICollection<MenuDto> Children { get; set; }
    public string Icon { get; set; }
    public bool IsSelectable { get; set; }
    public bool IsDisabled { get; set; }
    public int Order { get; set; }
    public string[] Permissions { get; set; }
    public string Router { get; set; }
    public string GroupName { get; set; }
    public string ConcurrencyStamp { get; set; }
}