using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.FileInfoBases;

[Serializable]
public class FileCheckInput : IHasHash
{
    [Required]
    [DisplayName("File:Hash")]
    [DynamicMaxLength(typeof(FileConsts), nameof(FileConsts.HashMaxLength))]
    public string Hash { get; set; } = default!;

    public long Size { get; set; } = 0;
    public int ChunkSize { get; set; } = FileConsts.DefaultChunkSize;
    public Guid? FolderId { get; set; } = default!;
}