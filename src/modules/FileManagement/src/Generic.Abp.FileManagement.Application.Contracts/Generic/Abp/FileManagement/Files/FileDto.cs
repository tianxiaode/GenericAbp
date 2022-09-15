using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.Files;

public class FileDto: ExtensibleAuditedEntityDto,IFile
{
    public string Filename { get; set; }
    public string MimeType { get; set;}
    public string FileType { get; set;}
    public long Size { get; set;}
    public string Description { get; set;}
    public string Hash { get; set;}
    public string Path { get; set; }
}