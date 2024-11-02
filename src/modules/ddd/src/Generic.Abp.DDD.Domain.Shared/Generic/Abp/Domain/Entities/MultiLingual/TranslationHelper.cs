using System.Globalization;

namespace Generic.Abp.Domain.Entities.MultiLingual
{
    public static class TranslationHelper
    {
        public static TTranslation? GetCurrentTranslation<TTranslation>(ICollection<TTranslation> translations)
            where TTranslation : ITranslation
        {
            return translations.FirstOrDefault(m =>
                m.Language.Equals(CultureInfo.CurrentCulture.Name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}