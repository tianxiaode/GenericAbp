using System;

namespace Generic.Abp.FileManagement.Resources;

[Serializable]
public class ResourceCacheItem(Guid id, string code, string name)
{
    public Guid Id { get; set; } = id;
    public string Code { get; set; } = code;
    public string Name { get; set; } = name;
}