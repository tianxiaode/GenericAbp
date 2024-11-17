using System;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuGetListInput
{
    public Guid? ParentId { get; set; } = default!;
    public string? Filter { get; set; } = default!;
}