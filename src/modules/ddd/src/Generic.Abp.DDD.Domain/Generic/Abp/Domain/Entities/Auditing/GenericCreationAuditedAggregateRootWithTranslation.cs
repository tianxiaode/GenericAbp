using System;
using Volo.Abp.Data;

namespace Generic.Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class GenericCreationAuditedAggregateRootWithTranslation : GenericCreationAuditedAggregateRoot,
        IHasExtraProperties
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; } = new();

        protected GenericCreationAuditedAggregateRootWithTranslation()
        {
        }

        protected GenericCreationAuditedAggregateRootWithTranslation(Guid id) : base(id)
        {
        }
    }
}