using System;

namespace Generic.Abp.FileManagement.Settings;

[Serializable]
public class FolderSetting
{
    public long StorageQuota { get; set; }
    public long MaxFileSize { get; set; }
    public string AllowFileTypes { get; set; } = default!;

    public FolderSetting()
    {
    }

    public FolderSetting(long storageQuota, long maxFileSize, string allowFileTypes)
    {
        StorageQuota = storageQuota;
        MaxFileSize = maxFileSize;
        AllowFileTypes = allowFileTypes;
    }
}