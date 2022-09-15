using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.Domain.Entities
{
    [Serializable]
    public abstract class GenericAggregateRoot : BasicAggregateRoot<Guid>, IHasConcurrencyStamp
    {
        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        protected GenericAggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected GenericAggregateRoot(Guid id)
            : base(id)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

    }
}
