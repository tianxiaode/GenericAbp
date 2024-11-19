using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Files;

public class FileCheckResult : ICheckFileResult
{
    public string Hash { get; set; }
    public bool IsExits { get; set; } = false;
    public Dictionary<int, bool> Uploaded { get; set; } = default!;
    public IFileInfoBase File { get; set; } = default!;

    public FileCheckResult(string hash)
    {
        Hash = hash;
    }

    public void SetFile(IFileInfoBase file)
    {
        File = file;
        IsExits = true;
    }
}