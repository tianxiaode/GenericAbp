using System;
using Generic.Abp.Extensions.Extensions;
using Volo.Abp.Data;

namespace Generic.Abp.FileManagement.Resources;

public static class ResourceExtensions
{
    private const string StorageQuotaPropertyName = "storageQuota";
    private const string UsedStoragePropertyName = "usedStorage";
    private const string AllowedFileTypesPropertyName = "allowedFileTypes";
    private const string AllowedFileCountPropertyName = "allowedFileCount";
    private const string MaxFileSizePropertyName = "maxFileSize";
    private const string StartTimePropertyName = "startTime";
    private const string EndTimePropertyName = "endTime";

    public static void SetStorageQuota<TEntity>(this TEntity entity, long storageQuota)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(StorageQuotaPropertyName, storageQuota);
    }

    public static long GetStorageQuota<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(StorageQuotaPropertyName, -1);
    }

    public static void SetUsedStorage<TEntity>(this TEntity entity, long usedStorage)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(UsedStoragePropertyName, usedStorage);
    }

    public static long GetUsedStorage<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty<long>(UsedStoragePropertyName);
    }

    public static void SetAllowedFileTypes<TEntity>(this TEntity entity, string allowedFileTypes)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(AllowedFileTypesPropertyName, allowedFileTypes);
    }

    public static string GetAllowedFileTypes<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(AllowedFileTypesPropertyName, "") ?? "";
    }

    public static void SetMaxFileSize<TEntity>(this TEntity entity, long maxFileSize)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(MaxFileSizePropertyName, maxFileSize);
    }

    public static long GetMaxFileSize<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty<long>(MaxFileSizePropertyName, -1);
    }

    public static void SetStartTime<TEntity>(this TEntity entity, DateTime startTime)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(StartTimePropertyName, startTime);
    }

    public static DateTime? GetStartTime<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(StartTimePropertyName, "").Parse<DateTime?>();
    }

    public static void SetEndTime<TEntity>(this TEntity entity, DateTime endTime)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(EndTimePropertyName, endTime);
    }

    public static DateTime? GetEndTime<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(EndTimePropertyName, "").Parse<DateTime?>();
    }

    public static void SetAllowedFileCount<TEntity>(this TEntity entity, int allowedFileCount)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(AllowedFileCountPropertyName, allowedFileCount);
    }

    public static int GetAllowedFileCount<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(AllowedFileCountPropertyName, -1);
    }
}