using System;
using System.ComponentModel;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Resources.Dtos;

public class ResourcePermissionCreateOrUpdateDto
{
    public Guid? Id { get; set; }
    public string ConcurrencyStamp { get; set; } = default!;

    [DisplayName("Permission:ProviderName")]
    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.ProviderNameMaxLength))]
    public string ProviderName { get; set; } = default!;

    [DisplayName("Permission:ProviderKey")]
    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.ProviderKeyMaxLength))]
    public string? ProviderKey { get; set; } = default!;

    [DisplayName("Permission:Permissions")]
    public virtual int Permissions { get; set; } = 0;
}