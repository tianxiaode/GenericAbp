using System.Collections.Generic;
using System.Linq;

namespace Generic.Abp.Domain.Trees
{
    public static class TreeExtensions
    {
        public static void SetCode<TEntity>(this TEntity entity, string code)
            where TEntity : class, ITree<TEntity>
        {
            entity.Code = code;
        }

        public static void MoveTo<TEntity>(this TEntity entity, TEntity parent)
            where TEntity : class, ITree<TEntity>
        {
            parent.Children ??= new List<TEntity>();
            parent.Children.Add(entity);
            entity.Parent?.Children?.Remove(entity);

            entity.Parent = parent;
            entity.ParentId = parent.Id;
        }

        public static void ClearChildren<TEntity>(this TEntity entity)
            where TEntity : class, ITree<TEntity>
        {
            if(entity.Children == null) return;
            entity.Children.ToList().ForEach(child =>
            {
                child.ParentId = null;
            });
            entity.Children.Clear();
        }

    }
}