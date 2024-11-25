using System;
using Generic.Abp.FileManagement.Dtos;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Folders.Dtos;

[Serializable]
public class FolderPermissionDto
{
    public bool IsInheritPermissions { get; set; } = true;
    public List<PermissionDto> Permissions { get; set; } = [];

    public FolderPermissionDto()
    {
    }

    public FolderPermissionDto(bool isInheritPermissions)
    {
        IsInheritPermissions = isInheritPermissions;
    }
}