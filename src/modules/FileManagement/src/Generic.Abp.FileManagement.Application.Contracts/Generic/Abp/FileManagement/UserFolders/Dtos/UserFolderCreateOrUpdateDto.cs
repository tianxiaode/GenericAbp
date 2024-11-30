using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.FileManagement.Resources;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.UserFolders.Dtos;

[Serializable]
public class UserFolderCreateOrUpdateDto
{
    public Guid OwnerId { get; set; }

    [Required]
    [DisplayName("ResourceConfiguration:AllowedFileTypes")]
    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.AllowedFileTypesMaxLength))]
    public string AllowedFileTypes { get; set; } = default!;

    public long StorageQuota { get; set; } = 0;
    public long MaxFileSize { get; set; } = 0;
    public bool IsEnabled { get; set; } = true;
}