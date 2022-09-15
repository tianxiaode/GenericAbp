using System;
using System.Linq.Expressions;
using Generic.Abp.Domain.Entities;
using Volo.Abp.Specifications;

namespace Generic.Abp.Domain.Trees
{
    public class ParentSpecification<TEntity>: Specification<TEntity> where TEntity: class, ITree<TEntity>
    {
        public string Code { get; set; }
        
        public int Level { get; set; }

        public ParentSpecification(string code, int level = 1)
        {
            Code = code;
            Level = level;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            var maxCodeLength = (Level-1) * (TreeConsts.CodeUnitLength+1);
            return maxCodeLength > 0
                ? m => Code.StartsWith(m.Code) && m.Code != Code
                : m => Code.StartsWith(m.Code) && m.Code != Code && m.Code.Length > maxCodeLength;
        }
    }
}