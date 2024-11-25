using System;

namespace Generic.Abp.FileManagement.Settings.Result;

[Serializable]
public class FileSetting
{
    public FileUpdateSetting UpdateSetting { get; set; } = default!;
    public FileCleanupSetting CleanupSetting { get; set; } = default!;
}