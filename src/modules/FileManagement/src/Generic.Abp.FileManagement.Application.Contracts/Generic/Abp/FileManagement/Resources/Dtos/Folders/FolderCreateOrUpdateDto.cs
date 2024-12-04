using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Resources.Dtos.Folders;

[Serializable]
public class FolderCreateOrUpdateDto
{
    [DisplayName("Resource:Name")]
    [Required]
    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.NameMaxLength))]
    public string Name { get; set; } = default!;


    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.AllowedFileTypesMaxLength))]
    [DisplayName("Resource:AllowedFileTypes")]
    public string? AllowedFileTypes { get; protected set; } = default!;

    [DisplayName("Resource:Quota")] public string? Quota { get; protected set; } = default!;

    [DisplayName("Resource:MaxFileSize")] public string? MaxFileSize { get; protected set; } = default!;
}