using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuDto : ExtensibleAuditedEntityDto<Guid>
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid? ParentId { get; set; } = default!;
    public bool Leaf { get; set; } = default!;
    public string? Icon { get; set; } = default!;
    public bool IsEnabled { get; set; } = default!;
    public bool IsStatic { get; set; } = default!;
    public int Order { get; set; } = default!;
    public string? Router { get; set; } = default!;
    public string GroupName { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;
    public MenuDto? Parent { get; set; } = default!;
    public List<MenuDto>? Children { get; set; } = default!;
    public Dictionary<string, object>? MultiLingual { get; set; } = new();
}