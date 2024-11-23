namespace Generic.Abp.FileManagement.FileInfoBases;

public enum FileRetentionPolicy
{
    Default = 0, // 未标记，遵循系统默认策略
    Retain = 1, // 标记为保留，永不清理
    Temporary = 2, //
}