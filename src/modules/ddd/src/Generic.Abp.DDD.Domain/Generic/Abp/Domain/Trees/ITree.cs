using Generic.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.Domain.Trees
{
    public interface ITree<TEntity> : IEntity<Guid>, ITree
        where TEntity : class, ITree<TEntity>
    {
        public TEntity? Parent { get; set; }
        public ICollection<TEntity>? Children { get; set; }
    }
}