using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathCreateOrUpdateDto
{
    [Required]
    [DisplayName("VirtualPath:Folder")]
    public Guid FolderId { get; set; } = default!;


    [Required]
    [DisplayName("VirtualPath:Path")]
    [DynamicMaxLength(typeof(VirtualPathConsts), nameof(VirtualPathConsts.PathMaxLength))]
    public string Path { get; set; } = default!;
}