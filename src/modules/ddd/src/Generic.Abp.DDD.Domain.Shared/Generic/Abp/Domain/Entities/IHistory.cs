using System;

namespace Generic.Abp.Domain.Entities
{
    public interface IHistory<out TEntityData> where TEntityData : class
    {
        Guid Id { get; }
        Guid EntityId { get; }
        DateTime ModificationTime { get; }
        string SearchKey { get; }
        TEntityData EntityData { get; }

    }
}
