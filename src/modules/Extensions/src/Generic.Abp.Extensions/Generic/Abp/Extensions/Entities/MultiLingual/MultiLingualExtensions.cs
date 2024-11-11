using System.Collections.Generic;
using System.Security;
using System.Text.Json;
using Volo.Abp.Data;

namespace Generic.Abp.Extensions.Entities.Multilingual;

public static class MultilingualExtensions
{
    private const string MultilingualPropertyName = "Multilingual";

    public static void SetMultilingual<TEntity>(this TEntity entity, Dictionary<string, object> languages)
        where TEntity : IHasExtraProperties
    {
        entity.SetProperty(MultilingualPropertyName, languages);
    }

    public static Dictionary<string, object> GetMultilingual<TEntity>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        var multilingual = entity.GetProperty(MultilingualPropertyName);
        if (multilingual == null)
        {
            return new Dictionary<string, object>();
        }

        return JsonSerializer.Deserialize<Dictionary<string, object>>(((JsonElement)multilingual).GetRawText()) ?? [];
    }
}