using System;

namespace Generic.Abp.FileManagement.Settings.Result;

[Serializable]
public class SettingResult
{
    public string StoragePath { get; set; } = default!;
    public int FolderCopyMaxNodeCount { get; set; } = default!;
    public FolderSetting PublicFolder { get; set; } = default!;
    public FolderSetting SharedFolder { get; set; } = default!;
    public FolderSetting UsersFolder { get; set; } = default!;
    public FolderSetting VirtualPath { get; set; } = default!;
    public FileSetting DefaultFile { get; set; } = default!;
    public FileSetting TemporaryFile { get; set; } = default!;
}