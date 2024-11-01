using System.Collections.Generic;
using Volo.Abp.Data;

namespace Generic.Abp.Extensions.Entities.MultiLingual;

public static class MultiLingualExtensions
{
    private const string MultiLingualPropertyName = "MultiLingual";

    public static void SetMultiLingual<TEntity>(this TEntity entity, Dictionary<string, object> languages)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(MultiLingualPropertyName, languages);
    }

    public static Dictionary<string, object> GetMultiLingual<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        return entity.GetProperty<Dictionary<string, object>>(MultiLingualPropertyName) ??
               new Dictionary<string, object>();
    }
}