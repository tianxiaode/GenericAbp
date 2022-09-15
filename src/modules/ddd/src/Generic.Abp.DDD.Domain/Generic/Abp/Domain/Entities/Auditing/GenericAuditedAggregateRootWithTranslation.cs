using System;
using Volo.Abp.Auditing;

namespace Generic.Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class
        GenericAuditedAggregateRootWithTranslation :
            GenericCreationAuditedAggregateRootWithTranslation,
            IAuditedObject
    {
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }

        protected GenericAuditedAggregateRootWithTranslation()
        {
        }

        protected GenericAuditedAggregateRootWithTranslation(Guid id) : base(id)
        {
        }
    }

}
