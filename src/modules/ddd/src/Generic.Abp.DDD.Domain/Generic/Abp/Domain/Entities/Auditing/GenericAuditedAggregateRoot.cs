using System;
using Volo.Abp.Auditing;

namespace Generic.Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class GenericAuditedAggregateRoot : GenericCreationAuditedAggregateRoot, IAuditedObject
    {
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual Guid? LastModifierId { get; set; }

        protected GenericAuditedAggregateRoot()
        {
        }

        protected GenericAuditedAggregateRoot(Guid id) : base(id)
        {
        }
    }
}
