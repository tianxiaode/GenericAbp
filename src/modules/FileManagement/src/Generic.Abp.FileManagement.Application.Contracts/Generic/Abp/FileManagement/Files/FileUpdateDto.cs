using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.FileManagement.FileInfoBases;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FileUpdateDto : IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; } = default!;

    [Required]
    [DisplayName("File:Filename")]
    [DynamicMaxLength(typeof(FileConsts), nameof(FileConsts.FilenameMaxLength))]
    public string Filename { get; set; } = default!;

    [DisplayName("File:Description")]
    [DynamicMaxLength(typeof(FileConsts), nameof(FileConsts.DescriptionMaxLength))]
    public string Description { get; set; } = default!;
}