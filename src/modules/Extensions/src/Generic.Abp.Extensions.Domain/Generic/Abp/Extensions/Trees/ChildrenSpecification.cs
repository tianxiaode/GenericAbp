using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace Generic.Abp.Extensions.Trees
{
    public class ChildrenSpecification<TEntity> : Specification<TEntity> where TEntity : class, ITree<TEntity>
    {
        public string Code { get; set; }

        public Guid? ParentId { get; set; }

        public ChildrenSpecification(string code = "", Guid? parentId = null)
        {
            Code = code;
            ParentId = parentId;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            if (ParentId.HasValue) return m => m.ParentId == ParentId;
            if (string.IsNullOrEmpty(Code)) return m => m.Code.StartsWith(Code) && m.Code != Code;
            return m => false;
        }
    }
}