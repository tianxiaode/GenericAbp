using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FileMergeInput : IHasHash
{
    [Required]
    [DisplayName("File:Hash")]
    [DynamicMaxLength(typeof(FileConsts), nameof(FileConsts.HashMaxLength))]
    public string Hash { get; set; } = default!;

    [Required]
    [DisplayName("File:Filename")]
    [DynamicMaxLength(typeof(FileConsts), nameof(FileConsts.FilenameMaxLength))]
    public string Filename { get; set; } = default!;

    public int TotalChunks { get; set; } = 0;
}