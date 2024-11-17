using System;

namespace Generic.Abp.FileManagement.Folders;

[Serializable]
public class FolderCacheItem(Guid id, string code, string name)
{
    public Guid Id { get; set; } = id;
    public string Code { get; set; } = code;
    public string Name { get; set; } = name;
}