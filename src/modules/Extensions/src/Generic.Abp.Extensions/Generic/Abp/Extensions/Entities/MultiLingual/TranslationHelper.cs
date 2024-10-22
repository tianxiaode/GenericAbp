using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Generic.Abp.Extensions.Entities.MultiLingual
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