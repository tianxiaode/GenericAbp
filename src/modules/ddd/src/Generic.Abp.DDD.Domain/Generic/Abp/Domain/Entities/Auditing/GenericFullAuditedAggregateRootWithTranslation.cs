using System;
using Volo.Abp.Auditing;

namespace Generic.Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class
        GenericFullAuditedAggregateRootWithTranslation :
            GenericAuditedAggregateRootWithTranslation,
            IFullAuditedObject 
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual Guid? DeleterId { get; set; }

        protected GenericFullAuditedAggregateRootWithTranslation()
        {
        }

        protected GenericFullAuditedAggregateRootWithTranslation(Guid id) : base(id)
        {
        }
    }
}
