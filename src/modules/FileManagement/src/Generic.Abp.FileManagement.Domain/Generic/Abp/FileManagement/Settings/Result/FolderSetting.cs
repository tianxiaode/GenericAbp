using System;

namespace Generic.Abp.FileManagement.Settings.Result;

[Serializable]
public class FolderSetting
{
    public string Quota { get; set; } = default!;
    public string FileMaxSize { get; set; } = default!;
    public string FileTypes { get; set; } = default!;
}