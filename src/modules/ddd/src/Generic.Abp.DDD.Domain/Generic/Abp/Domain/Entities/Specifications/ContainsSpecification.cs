using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Specifications;

namespace Generic.Abp.Domain.Entities.Specifications
{
    public class ContainsSpecification<TEntity> : Specification<TEntity> where TEntity : Entity<Guid>
    {
        public List<Guid> Ids { get; set; }

        public ContainsSpecification(List<Guid> ids)
        {
            Ids = ids;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return m => Ids.Contains(m.Id);
        }
    }
}
