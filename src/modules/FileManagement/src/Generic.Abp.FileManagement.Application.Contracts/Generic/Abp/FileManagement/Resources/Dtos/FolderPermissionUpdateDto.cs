using System;
using System.Collections.Generic;
using Generic.Abp.FileManagement.Dtos;

namespace Generic.Abp.FileManagement.Folders.Dtos;

[Serializable]
public class FolderPermissionUpdateDto
{
    public bool IsInheritPermissions { get; set; } = true;
    public List<PermissionCreateOrUpdateDto> Permissions { get; set; } = [];
}