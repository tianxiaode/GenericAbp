using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Generic.Abp.Extensions.Trees
{
    public class TreeManager<TEntity, TRepository> : DomainService
        where TEntity : class, ITree<TEntity>
        where TRepository : IRepository<TEntity, Guid>
    {
        protected TRepository Repository { get; }
        protected ITreeCodeGenerator<TEntity> TreeCodeGenerator { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }
        protected CancellationToken CancellationToken => CancellationTokenProvider.Token;

        public TreeManager(
            TRepository repository,
            ITreeCodeGenerator<TEntity> treeCodeGenerator,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            Repository = repository;
            TreeCodeGenerator = treeCodeGenerator;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(TEntity entity, bool autoSave = true, bool updatePermissions = true)
        {
            entity.SetCode(await GetNextChildCodeAsync(entity.ParentId));
            await ValidateAsync(entity);
            await Repository.InsertAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true, bool updatePermissions = true)
        {
            await ValidateAsync(entity);
            await Repository.UpdateAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            await Repository.DeleteAsync(id, true, cancellationToken: CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var entity = await Repository.GetAsync(id, cancellationToken: CancellationToken);
            if (entity.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of entity
            var oldCode = entity.Code;

            //Move
            entity.SetCode(await GetNextChildCodeAsync(parentId));
            entity.MoveTo(parentId);

            await ValidateAsync(entity);

            //Update Children Codes
            foreach (var child in children)
            {
                child.SetCode(TreeCodeGenerator.Append(entity.Code,
                    TreeCodeGenerator.GetRelative(child.Code, oldCode)));
            }
        }


        [UnitOfWork]
        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null)
            {
                return TreeCodeGenerator.Next(lastChild.Code);
            }

            var parentCode = parentId != null
                ? await GetCodeOrDefaultAsync(parentId.Value)
                : null;

            return TreeCodeGenerator.Append(
                parentCode ?? "",
                TreeCodeGenerator.Create(1)
            );
        }

        [UnitOfWork]
        public virtual async Task<TEntity?> GetLastChildOrNullAsync(Guid? parentId)
        {
            if (!parentId.HasValue)
            {
                return default;
            }

            var children =
                await Repository.GetListAsync(m => m.ParentId == parentId, cancellationToken: CancellationToken);
            return children.MaxBy(c => c.Code);
        }

        public virtual async Task ValidateAsync(TEntity entity)
        {
            //判断是否存在父节点
            if (entity.ParentId.HasValue)
            {
                await Repository.GetAsync(entity.ParentId.Value, false, CancellationToken);
            }
        }

        [UnitOfWork]
        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, cancellationToken: CancellationToken);
            return entity.Code;
        }

        [UnitOfWork]
        public virtual async Task<List<TEntity>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            var query = await Repository.GetQueryableAsync();
            if (!recursive)
            {
                return await AsyncExecuter.ToListAsync(
                    query.Where(new ChildrenSpecification<TEntity>(parentId: parentId)), CancellationToken);
            }

            if (!parentId.HasValue)
            {
                return await Repository.GetListAsync(includeDetails: false, cancellationToken: CancellationToken);
            }

            var code = await GetCodeOrDefaultAsync(parentId.Value);

            return await AsyncExecuter.ToListAsync(query.Where(new ChildrenSpecification<TEntity>(code)),
                CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task<List<TEntity>> FindParentAsync(string code, int level = 1)
        {
            var query = await Repository.GetQueryableAsync();
            return await AsyncExecuter.ToListAsync(query.Where(new ParentSpecification<TEntity>(code, level)),
                CancellationToken);
        }
    }
}