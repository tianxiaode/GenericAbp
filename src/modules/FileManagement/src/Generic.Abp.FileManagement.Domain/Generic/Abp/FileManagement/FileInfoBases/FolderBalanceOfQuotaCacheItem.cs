using System;

namespace Generic.Abp.FileManagement.FileInfoBases;

[Serializable()]
public class FolderBalanceOfQuotaCacheItem
{
    public long Balance { get; set; } = 0;
}