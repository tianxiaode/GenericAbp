using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.Files;

public class FileCheckInput: IHasHash
{
    [Required]
    public string Hash { get; set; }
    public long Size { get; set; } = 0;
    public int ChunkSize { get; set; } = FileConsts.DefaultChunkSize;
}