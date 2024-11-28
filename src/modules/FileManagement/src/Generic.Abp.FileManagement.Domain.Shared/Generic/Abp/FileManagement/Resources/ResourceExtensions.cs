using Volo.Abp.Data;

namespace Generic.Abp.FileManagement.Resources;

public static class ResourceExtensions
{
    private const string QuotaPropertyName = "quota";
    private const string UsedSizePropertyName = "usedSize";
    private const string AllowedFileTypesPropertyName = "allowedFileTypes";
    private const string MaxFileSizePropertyName = "maxFileSize";

    public static void SetQuota<TEntity>(this TEntity entity, string quota) where TEntity : IHasExtraProperties
    {
        entity.SetProperty(QuotaPropertyName, quota);
    }

    public static string GetQuota<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(QuotaPropertyName, ResourceConsts.UserFolder.DefaultQuota) ??
               ResourceConsts.UserFolder.DefaultQuota;
    }

    public static void SetUsedSize<TEntity>(this TEntity entity, long usedSize) where TEntity : IHasExtraProperties
    {
        entity.SetProperty(UsedSizePropertyName, usedSize);
    }

    public static long GetUsedSize<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty<long>(UsedSizePropertyName, 0);
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

    public static void SetMaxFileSize<TEntity>(this TEntity entity, string maxFileSize)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(MaxFileSizePropertyName, maxFileSize);
    }

    public static string GetMaxFileSize<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty(MaxFileSizePropertyName, ResourceConsts.UserFolder.DefaultFileMaxSize) ??
               ResourceConsts.UserFolder.DefaultFileMaxSize;
    }
}