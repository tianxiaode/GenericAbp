using System;
using Volo.Abp.Auditing;

namespace Generic.Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class GenericFullAuditedAggregateRoot : GenericAuditedAggregateRoot, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
        public virtual Guid? DeleterId { get; set; }

        protected GenericFullAuditedAggregateRoot()
        {
        }

        protected GenericFullAuditedAggregateRoot(Guid id) : base(id)
        {
        }
    }
}
