using System;

namespace Generic.Abp.FileManagement.Settings.Result;

[Serializable]
public class FileUpdateSetting
{
    public bool Enable { get; set; } = default!;

    public int RetentionPeriod { get; set; } = default!;
    public int Frequency { get; set; } = default!;
    public int BatchSize { get; set; } = default!;
}