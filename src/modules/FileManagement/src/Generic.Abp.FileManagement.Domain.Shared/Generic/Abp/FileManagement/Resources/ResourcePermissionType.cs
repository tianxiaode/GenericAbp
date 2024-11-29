using System;

namespace Generic.Abp.FileManagement.Resources;

[Flags]
public enum ResourcePermissionType
{
    None = 0,
    CanRead = 1 << 0,
    CanWrite = 1 << 1,
    CanDelete = 1 << 2,
}