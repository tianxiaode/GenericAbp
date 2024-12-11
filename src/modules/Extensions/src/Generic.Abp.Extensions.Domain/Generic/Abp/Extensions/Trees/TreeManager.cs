using Generic.Abp.Extensions.Entities;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Threading;

namespace Generic.Abp.Extensions.Trees
{
    public abstract partial class TreeManager<TEntity, TRepository, TResource>(
        TRepository repository,
        ITreeCodeGenerator<TEntity> treeCodeGenerator,
        IStringLocalizer<TResource> localizer,
        ICancellationTokenProvider cancellationTokenProvider)
        : EntityManagerBase<TEntity, TRepository, TResource>(repository, localizer, cancellationTokenProvider)
        where TEntity : TreeAuditedAggregateRoot<TEntity>
        where TRepository : ITreeRepository<TEntity>
        where TResource : class
    {
        protected ITreeCodeGenerator<TEntity> TreeCodeGenerator { get; } = treeCodeGenerator;

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            return await Repository.GetAsync(id, false, CancellationToken);
        }

        public override async Task CreateAsync(TEntity entity, bool autoSave = true)
        {
            entity.SetCode(await GetNextChildCodeAsync(entity.ParentId));
            await ValidateAsync(entity);
            await Repository.InsertAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        public override async Task UpdateAsync(TEntity entity, bool autoSave = true)
        {
            await ValidateAsync(entity);
            await Repository.UpdateAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        public override async Task DeleteAsync(Guid id, bool autoSave = true)
        {
            var entity = await Repository.GetAsync(id, false, CancellationToken);
            if (!await BeforeDeleteAsync(entity))
            {
                return;
            }

            await DeleteAsync(entity, autoSave);
            await AfterDeleteAsync(entity);
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = true)
        {
            await Repository.DeleteAllChildrenByCodeAsync(entity.Code, CancellationToken);
            await Repository.DeleteAsync(entity, true, CancellationToken);
        }

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
            if (codes.Count == 0)
            {
                return [];
            }


            var allParentCodes = await GetAllParentCodes(codes);

            return await Repository.GetListAsync(m => allParentCodes.Contains(m.Code), false, CancellationToken);
        }

        public virtual Task<HashSet<string>> GetAllParentCodes(List<string> codes)
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


        protected virtual Task<bool> BeforeDeleteAsync(TEntity entity)
        {
            return Task.FromResult(true);
        }

        protected virtual Task AfterDeleteAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}