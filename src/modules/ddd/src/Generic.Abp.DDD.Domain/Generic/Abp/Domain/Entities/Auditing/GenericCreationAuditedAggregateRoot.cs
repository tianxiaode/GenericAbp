using System;
using Volo.Abp.Auditing;

namespace Generic.Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class GenericCreationAuditedAggregateRoot : GenericAggregateRoot, ICreationAuditedObject
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid? CreatorId { get; set; }

        protected GenericCreationAuditedAggregateRoot()
        {
        }

        protected GenericCreationAuditedAggregateRoot(Guid id) : base(id)
        {
        }
    }
}
