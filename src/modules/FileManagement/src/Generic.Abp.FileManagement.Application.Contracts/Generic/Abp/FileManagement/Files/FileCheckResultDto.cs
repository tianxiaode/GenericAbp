using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Files;

public class FileCheckResultDto: ICheckFileResult
{
    public string Hash { get; set; }
    public bool IsExits { get; set;}
    public IFile File { get; set; }
    public Dictionary<int, bool> Uploaded { get; set;}
}