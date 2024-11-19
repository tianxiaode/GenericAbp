using System;
using System.Collections.Generic;
using Generic.Abp.FileManagement.Dtos;

namespace Generic.Abp.FileManagement.Files;

[Serializable]
public class FilePermissionDto
{
    public bool IsInheritPermissions { get; set; } = true;
    public List<PermissionDto> Permissions { get; set; } = [];

    public FilePermissionDto()
    {
    }

    public FilePermissionDto(bool isInheritPermissions)
    {
        IsInheritPermissions = isInheritPermissions;
    }
}