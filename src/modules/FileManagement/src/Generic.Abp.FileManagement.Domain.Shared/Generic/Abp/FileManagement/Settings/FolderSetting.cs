using System;

namespace Generic.Abp.FileManagement.Settings;

[Serializable]
public class FolderSetting
{
    public long StorageQuota { get; set; }
    public long MaxFileSize { get; set; }
    public string AllowFileTypes { get; set; } = default!;
    public int AllowFileCount { get; set; }

    public FolderSetting()
    {
    }

    public FolderSetting(long storageQuota, long maxFileSize, string allowFileTypes, int allowFileCount)
    {
        StorageQuota = storageQuota;
        MaxFileSize = maxFileSize;
        AllowFileTypes = allowFileTypes;
        AllowFileCount = allowFileCount;
    }
}