using Generic.Abp.FileManagement.Resources.Dtos;
using System;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathDto : ResourceBaseDto
{
    public virtual Guid? FolderId { get; protected set; }
    public virtual ResourceBaseDto? Folder { get; protected set; }
}