using System;
using System.Collections.Generic;
using Generic.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.Domain.Trees
{
    public interface ITree<TEntity> : IEntity<Guid> ,ITree
        where TEntity : class, ITree<TEntity>
    {
        TEntity? Parent { get; set; }
        ICollection<TEntity>? Children { get; set; }
    }
}
