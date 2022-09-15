using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace Generic.Abp.Domain.Entities.Specifications
{
    public class InOrganizationUnitSpecification<TEntity> : Specification<TEntity> where TEntity : class, IHasOrganizationUnit
    {
        public Guid Id { get; set; }

        public InOrganizationUnitSpecification(Guid id)
        {
            Id = id;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return m => m.OrganizationUnitId == Id;
        }

    }
}
