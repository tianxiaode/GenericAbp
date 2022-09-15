using System;
using Generic.Abp.Domain.Entities.MultiLingual;

namespace Generic.Abp.Domain.Entities
{
    [Serializable]
    public class Translation : ITranslation
    {
        public string Language { get; protected set; }

        protected Translation(string language)
        {
            Language = language;
        }
    }
}
