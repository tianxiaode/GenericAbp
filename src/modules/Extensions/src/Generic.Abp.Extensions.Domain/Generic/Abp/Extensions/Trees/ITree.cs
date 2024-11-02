using Generic.Abp.Extensions.Entities.Trees;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.Extensions.Trees
{
    public interface ITree<TEntity> : IEntity<Guid>, ITree
        where TEntity : class, ITree<TEntity>
    {
        public TEntity? Parent { get; protected set; }
        public ICollection<TEntity>? Children { get; protected set; }
    }
}