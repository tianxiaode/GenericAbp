using System;
using System.ComponentModel;
using Generic.Abp.FileManagement.VirtualPaths;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Dtos;

[Serializable]
public class PermissionCreateOrUpdateDto
{
    public Guid? Id { get; set; } = default!;
    public string ConcurrencyStamp { get; set; } = default!;

    [DisplayName("Permission:ProviderName")]
    [DynamicMaxLength(typeof(VirtualPathConsts), nameof(VirtualPathConsts.ProviderNameMaxLength))]
    public string ProviderName { get; set; } = default!;

    [DisplayName("Permission:ProviderKey")]
    [DynamicMaxLength(typeof(VirtualPathConsts), nameof(VirtualPathConsts.ProviderKeyMaxLength))]
    public string? ProviderKey { get; set; } = default!;

    [DisplayName("Permission:CanRead")] public bool CanRead { get; set; }
    [DisplayName("Permission:CanWrite")] public bool CanWrite { get; set; }
    [DisplayName("Permission:CanDelete")] public bool CanDelete { get; set; }
}