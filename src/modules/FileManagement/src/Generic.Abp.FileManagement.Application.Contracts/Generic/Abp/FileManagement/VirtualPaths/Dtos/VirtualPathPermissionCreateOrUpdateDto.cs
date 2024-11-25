using System;
using System.Collections.Generic;
using Generic.Abp.FileManagement.Dtos;

namespace Generic.Abp.FileManagement.VirtualPaths.Dtos;

[Serializable]
public class VirtualPathPermissionCreateOrUpdateDto
{
    public List<PermissionCreateOrUpdateDto> Permissions { get; set; } = [];
}