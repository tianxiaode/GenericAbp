using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.MenuManagement.Menus.Dtos;

[Serializable]
public class MenuDto : ExtensibleAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
{
    public Guid? TenantId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public Guid? ParentId { get; set; } = default!;
    public bool Leaf { get; set; } = default!;
    public string? Icon { get; set; } = default!;
    public bool IsEnabled { get; set; } = default!;
    public bool IsStatic { get; set; } = default!;
    public int Order { get; set; } = default!;
    public string? Router { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;
    public MenuDto? Parent { get; set; } = default!;
    public List<MenuDto>? Children { get; set; } = default!;
    public Dictionary<string, object>? Multilingual { get; set; } = new();
    public List<string>? Permissions { get; set; } = default!;
}