using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Folders.Dtos;

[Serializable]
public class FolderCreateOrUpdateDto
{
    [DisplayName("Folder:Name")]
    [Required]
    [DynamicMaxLength(typeof(FolderConsts), nameof(FolderConsts.NameMaxLength))]
    public string Name { get; set; } = default!;

    [DisplayName("Folder:IsInheritPermissions")]
    public bool IsInheritPermissions { get; protected set; } = true;

    [Required]
    [DynamicMaxLength(typeof(FolderConsts), nameof(FolderConsts.AllowedFileTypesMaxLength))]
    [DisplayName("Folder:AllowedFileTypes")]
    public string AllowedFileTypes { get; protected set; } = default!;

    [Required]
    [DisplayName("Folder:StorageQuota")]
    public long StorageQuota { get; protected set; } = 0;

    [Required]
    [DisplayName("Folder:UsedStorage")]
    public long UsedStorage { get; protected set; } = 0;

    [Required]
    [DisplayName("Folder:MaxFileSize")]
    public long MaxFileSize { get; protected set; } = 0;
}