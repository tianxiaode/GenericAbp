using System;

namespace Generic.Abp.FileManagement.Settings.Result;

[Serializable]
public class FolderSetting
{
    public long StorageQuota { get; set; } = default!;
    public long MaxFileSize { get; set; } = default!;
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