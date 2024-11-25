using Generic.Abp.Extensions.Entities.Trees;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.Extensions.Trees
{
    public interface ITreeEntity : IEntity<Guid>, IMultiTenant, ITree
    {
    }

    public interface ITree<TEntity> : ITreeEntity
        where TEntity : class, ITree<TEntity>
    {
        public TEntity? Parent { get; protected set; }
        public ICollection<TEntity>? Children { get; protected set; }
    }
}