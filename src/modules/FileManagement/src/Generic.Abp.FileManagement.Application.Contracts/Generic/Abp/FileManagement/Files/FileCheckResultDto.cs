using System;
using System.Collections.Generic;
using Generic.Abp.FileManagement.FileInfoBases;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FileCheckResultDto : ICheckFileResult
{
    public string Hash { get; set; } = default!;
    public bool IsExits { get; set; } = default!;
    public IFileInfoBase File { get; set; } = default!;
    public Dictionary<int, bool> Uploaded { get; set; } = default!;
}