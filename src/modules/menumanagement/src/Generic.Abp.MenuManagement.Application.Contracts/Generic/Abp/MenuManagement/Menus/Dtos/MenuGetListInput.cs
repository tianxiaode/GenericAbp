using System;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuGetListInput
{
    public Guid? PrentId { get; set; } = default!;
    public string? Filter { get; set; } = default!;
    public string? GroupName { get; set; } = default!;
    public string? Sorting { get; set; } = default!;
}