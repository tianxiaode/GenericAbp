using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Files;

public class FileCheckResult: ICheckFileResult
{
    public string Hash { get; set; }
    public bool IsExits { get; set; }
    public Dictionary<int, bool> Uploaded { get; set; }
    public IFile File { get; set; }

    public FileCheckResult(string hash)
    {
        Hash = hash;
    }

    public void SetFile(IFile file)
    {
        File = file;
        IsExits = true;
    }
}