using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathCreateOrUpdateDto
{
    [Required]
    [DisplayName("VirtualPath:Folder")]
    public Guid ResourceId { get; set; } = default!;

    [Required]
    [DisplayName("VirtualPath:Name")]
    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.NameMaxLength))]
    public string Name { get; set; } = default!;

    [DisplayName("VirtualPath:IsAccessible")]
    public bool IsAccessible { get; set; } = true;

    [DisplayName("VirtualPath:StartTime")] public DateTime? StartTime { get; set; } = default!;

    [DisplayName("VirtualPath:EndTime")] public DateTime? EndTime { get; set; } = default!;
}