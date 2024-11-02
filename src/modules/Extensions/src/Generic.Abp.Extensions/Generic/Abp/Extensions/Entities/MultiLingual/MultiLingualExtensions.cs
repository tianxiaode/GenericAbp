using System.Collections.Generic;
using System.Security;
using System.Text.Json;
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
        var multiLingual = entity.GetProperty(MultiLingualPropertyName);
        if (multiLingual == null)
        {
            return new Dictionary<string, object>();
        }

        return JsonSerializer.Deserialize<Dictionary<string, object>>(((JsonElement)multiLingual).GetRawText()) ?? [];
    }
}