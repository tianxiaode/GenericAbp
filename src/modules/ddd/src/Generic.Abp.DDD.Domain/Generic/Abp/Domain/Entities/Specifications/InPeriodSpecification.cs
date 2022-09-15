using System;
using System.Linq.Expressions;
using Volo.Abp.Specifications;

namespace Generic.Abp.Domain.Entities.Specifications
{
    public class InPeriodSpecification<TEntity> : Specification<TEntity> where TEntity : class, ITimeLimit
    {
        public DateTime Current { get; set; }

        public InPeriodSpecification(DateTime current)
        {
            Current = current.Date;
        }

        public override Expression<Func<TEntity, bool>> ToExpression()
        {
            return m => m.EndTime.Year == 9999 || (m.StartTime >= Current && Current <= m.EndTime);
        }
    }
}
