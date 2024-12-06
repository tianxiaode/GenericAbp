using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Generic.Abp.FileManagement.Resources;
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

    [Required]
    [DisplayName("File:Filename")]
    [DynamicMaxLength(typeof(ResourceConsts), nameof(ResourceConsts.NameMaxLength))]
    public string Filename { get; set; } = default!;

    public byte[] FirstChunk { get; set; } = default!;
    public string ChunkHash { get; set; } = default!;

    public Guid? FolderId { get; set; } = default!;
}