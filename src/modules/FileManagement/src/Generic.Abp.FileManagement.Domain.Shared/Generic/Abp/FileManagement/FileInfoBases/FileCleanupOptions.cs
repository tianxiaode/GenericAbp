using System;

namespace Generic.Abp.FileManagement.FileInfoBases;

public class FileCleanupOptions
{
    public TimeSpan DefaultRetentionPeriod { get; set; } = TimeSpan.FromDays(30); // 默认保留 30 天
    public TimeSpan TemporaryRetentionPeriod { get; set; } = TimeSpan.FromDays(7); // 临时文件保留 7 天
    public bool EnableCleanup { get; set; } = true; // 是否启用清理功能
}