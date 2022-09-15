using System.Collections.Generic;
using Volo.Abp.Data;

namespace Generic.Abp.Domain.Extensions;

public static class TranslationExtensions
{
    private const string TranslationsPropertyName = "Translations";

    public static void SetTranslations<TEntity,TTranslation>(this TEntity entity, IEnumerable<TTranslation> translations) where  TEntity: IHasExtraProperties
    {
        entity.SetProperty(TranslationsPropertyName, System.Text.Json.JsonSerializer.Serialize(translations));
    }

    public static List<TTranslation> GetTranslations<TEntity, TTranslation>(this TEntity entity)
        where TEntity : IHasExtraProperties
    {
        var str = entity.GetProperty<string>(TranslationsPropertyName);
        return string.IsNullOrEmpty(str)
            ? new List<TTranslation>()
            : System.Text.Json.JsonSerializer.Deserialize<List<TTranslation>>(str) ?? new List<TTranslation>();
    }
}