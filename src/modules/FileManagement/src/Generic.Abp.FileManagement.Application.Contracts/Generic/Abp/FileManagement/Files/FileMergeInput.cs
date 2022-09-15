using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.Files;

public class FileMergeInput: IHasHash
{
    [Required]
    public string Hash { get; set; }

    [Required]
    public string Filename { get; set; }

    public int TotalChunks { get; set; } = 0;
}