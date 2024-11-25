using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace Generic.Abp.Extensions.Trees
{
    public class ChildrenSpecification<TEntity>(string code = "", Guid? parentId = null) : Specification<TEntity>
        where TEntity : class, ITree<TEntity>
    {
        public string Code { get; set; } = code;

        public Guid? ParentId { get; set; } = parentId;

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            if (ParentId.HasValue)
            {
                return m => m.ParentId == ParentId;
            }

            if (!string.IsNullOrEmpty(Code))
            {
                return m => m.Code.StartsWith(Code + ".");
            }

            return m => false;
        }
    }
}