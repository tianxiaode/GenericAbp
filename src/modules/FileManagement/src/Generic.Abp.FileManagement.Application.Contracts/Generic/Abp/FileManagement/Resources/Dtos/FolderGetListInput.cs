using System;

namespace Generic.Abp.FileManagement.Folders.Dtos;

[Serializable]
public class FolderGetListInput
{
    public Guid? FolderId { get; set; } = default!;
    public string? Filter { get; set; } = default!;
}