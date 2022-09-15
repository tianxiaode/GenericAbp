using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.Domain.Entities
{
    [Serializable]
    public abstract class HistoryEntity<TEntityData> : Entity<Guid>, IHistory<TEntityData> where TEntityData : class
    {
        public virtual Guid EntityId { get; protected set; }
        public virtual DateTime ModificationTime { get; protected set; }
        public virtual string SearchKey { get; protected set; }
        public virtual TEntityData EntityData { get; protected set; }

        protected HistoryEntity(Guid id, Guid entityId, DateTime modificationTime, TEntityData entityData) : base(id)
        {
            EntityId = entityId;
            ModificationTime = modificationTime;
            EntityData = entityData;

            SearchKey = HistoryKeyGenerator.Create(entityId, modificationTime);

        }
    }
}
