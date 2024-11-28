using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;

namespace Generic.Abp.Extensions.Trees
{
    public abstract partial class TreeManager<TEntity, TRepository>(
        TRepository repository,
        ITreeCodeGenerator<TEntity> treeCodeGenerator,
        ICancellationTokenProvider cancellationTokenProvider)
        : DomainService
        where TEntity : TreeAuditedAggregateRoot<TEntity>
        where TRepository : ITreeRepository<TEntity>
    {
        protected TRepository Repository { get; } = repository;
        protected ITreeCodeGenerator<TEntity> TreeCodeGenerator { get; } = treeCodeGenerator;
        protected ICancellationTokenProvider CancellationTokenProvider { get; } = cancellationTokenProvider;
        protected CancellationToken CancellationToken => CancellationTokenProvider.Token;


        public virtual async Task<List<TEntity>> FindChildrenAsync(TEntity entity, bool includeDetails = false,
            bool recursive = false)
        {
            if (!recursive)
            {
                return await Repository.GetListAsync(m => m.ParentId == entity.Id, includeDetails, CancellationToken);
            }


            return await Repository.GetListAsync(m => m.Code.StartsWith(entity.Code + "."), includeDetails,
                CancellationToken);
        }

        public virtual async Task<List<TEntity>> FindAllChildrenByCodeAsync(string code)
        {
            return await Repository.GetListAsync(m => m.Code.StartsWith(code + "."), false, CancellationToken);
        }

        public virtual async Task<bool> HasChildrenAsync(Guid id)
        {
            return await Repository.AnyAsync(m => m.ParentId == id, CancellationToken);
        }

        public virtual async Task<List<TEntity>> FindParentAsync(string code, int level = 1)
        {
            var query = await Repository.GetQueryableAsync();
            return await AsyncExecuter.ToListAsync(query.Where(new ParentSpecification<TEntity>(code, level)),
                CancellationToken);
        }

        public virtual async Task<List<TEntity>> GetSearchListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            //先找出所有符合条件的code
            var codes = await (await Repository.GetQueryableAsync()).Where(predicate).Select(m => m.Code)
                .ToDynamicListAsync<string>(CancellationToken);
            ;
            if (!codes.Any())
            {
                return [];
            }


            var allParents = await GetAllParents(codes);

            return await Repository.GetListAsync(m => allParents.Contains(m.Code), false, CancellationToken);
        }

        public virtual Task<HashSet<string>> GetAllParents(List<string> codes)
        {
            var parents = new HashSet<string>();
            foreach (var parts in codes.Select(code => code.Split('.')))
            {
                for (var i = 1; i <= parts.Length; i++)
                {
                    var parent = string.Join(".", parts.Take(i));
                    parents.Add(parent);
                }
            }

            return Task.FromResult(parents);
        }
    }
}