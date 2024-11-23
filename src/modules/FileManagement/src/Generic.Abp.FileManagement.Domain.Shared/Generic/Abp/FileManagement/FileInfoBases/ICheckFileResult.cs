using System.Collections.Generic;

namespace Generic.Abp.FileManagement.FileInfoBases;

public interface ICheckFileResult : IHasHash
{
    bool IsExits { get; }
    IFileInfoBase File { get; set; }
    Dictionary<int, bool> Uploaded { get; }
}