using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FileUploadChunkInput : IHasHash
{
    [Required]
    [DisplayName("File:Hash")]
    [DynamicMaxLength(typeof(FileConsts), nameof(FileConsts.HashMaxLength))]

    public string Hash { get; set; } = default!;

    public byte[] ChunkBytes { get; set; } = default!;
    public int Index { get; set; } = default!;
    public Guid FolderId { get; set; } = default!;
}