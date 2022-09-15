using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.Files;

public class FileUploadChunkInput:IHasHash
{
    [Required]
    public string Hash { get; set; }
    public byte[] ChunkBytes { get; set; }
    public int Index { get; set; }

}