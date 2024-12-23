﻿using System;
using System.Collections.Generic;
using Generic.Abp.FileManagement.Dtos;

namespace Generic.Abp.FileManagement.FileInfoBases;

[Serializable]
public class FilePermissionUpdateDto
{
    public bool IsInheritPermissions { get; set; } = true;
    public List<PermissionCreateOrUpdateDto> Permissions { get; set; } = [];
}