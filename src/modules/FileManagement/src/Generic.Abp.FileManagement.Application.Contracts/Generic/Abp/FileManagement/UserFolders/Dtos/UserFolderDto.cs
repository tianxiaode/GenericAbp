using Generic.Abp.FileManagement.Resources.Dtos;
using System;

namespace Generic.Abp.FileManagement.UserFolders.Dtos;

public class UserFolderDto : ResourceBaseDto
{
    public string AllowedFileTypes { get; set; } = default!;
    public long StorageQuota { get; set; } = 0;
    public long UsedStorage { get; set; } = 0;
    public long MaxFileSize { get; set; } = 0;
}