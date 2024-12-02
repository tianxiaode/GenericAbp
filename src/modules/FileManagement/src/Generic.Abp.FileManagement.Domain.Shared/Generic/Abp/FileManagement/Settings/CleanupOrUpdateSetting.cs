using System;

namespace Generic.Abp.FileManagement.Settings;

[Serializable]
public class CleanupOrUpdateSetting
{
    public bool Enable { get; set; }
    public int? RetentionPeriod { get; set; }
    public long Frequency { get; set; }
    public int BatchSize { get; set; }

    public CleanupOrUpdateSetting()
    {
    }

    public CleanupOrUpdateSetting(bool enable, long frequency, int batchSize, int? retentionPeriod)
    {
        Enable = enable;
        Frequency = frequency;
        BatchSize = batchSize;
        RetentionPeriod = retentionPeriod;
    }
}