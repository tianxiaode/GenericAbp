using System;

namespace Generic.Abp.FileManagement.Resources.Dtos;

[Serializable]
public class FolderGetListInput
{
    public Guid? ParentId { get; set; } = default!;
    public string? Filter { get; set; } = default!;
}