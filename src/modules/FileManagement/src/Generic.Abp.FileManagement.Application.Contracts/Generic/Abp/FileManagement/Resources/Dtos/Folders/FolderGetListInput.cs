using System;
using Generic.Abp.Extensions.Entities.QueryParams;

namespace Generic.Abp.FileManagement.Resources.Dtos.Folders;

[Serializable]
public class FolderGetListInput : IHasFilter
{
    public string? Filter { get; set; }
}