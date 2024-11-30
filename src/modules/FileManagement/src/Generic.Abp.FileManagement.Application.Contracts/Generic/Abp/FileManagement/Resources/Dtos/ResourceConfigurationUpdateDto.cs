using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class ResourceConfigurationUpdateDto : IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;

    [Required]
    [DisplayName("ResourceConfiguration:AllowedFileTypes")]
    public string AllowedFileTypes { get; set; } = default!;

    [DisplayName("ResourceConfiguration:StorageQuota")]
    public long StorageQuota { get; set; } = 0;

    [DisplayName("ResourceConfiguration:StorageQuota")]
    public long UsedStorage { get; set; } = 0;

    [DisplayName("ResourceConfiguration:MaxFileSize")]
    public long MaxFileSize { get; set; } = 0;
}