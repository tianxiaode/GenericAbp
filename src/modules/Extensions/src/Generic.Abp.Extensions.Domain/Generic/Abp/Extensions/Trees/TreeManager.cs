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
        public virtual async Task CreateAsync(TEntity entity, bool autoSave = true)
        {
            entity.SetCode(await GetNextChildCodeAsync(entity.ParentId));
            await ValidateAsync(entity);
            await Repository.InsertAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true)
        {
            await ValidateAsync(entity);
            await Repository.UpdateAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id, bool autoSave = true)
        {
            var entity = await Repository.GetAsync(id, false, CancellationToken);
            await DeleteAsync(entity, autoSave);
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = true)
        {
            await DeleteChildrenAsync(entity, autoSave);
            await Repository.DeleteAsync(entity, true, CancellationToken);
        }

        [UnitOfWork]
        public virtual async Task DeleteChildrenAsync(TEntity entity, bool autoSave = true)
        {
            var children = await Repository.GetListAsync(m => m.ParentId == entity.Id, false, CancellationToken);
            foreach (var child in children)
            {
                await DeleteChildrenAsync(child, autoSave);
                await DeleteAsync(child, autoSave);
            }
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var entity = await Repository.GetAsync(id, false, CancellationToken);
            if (entity.ParentId == parentId)
            {
                return;
            }

            await MoveAsync(entity, parentId);
        }


        [UnitOfWork]
        public virtual async Task MoveAsync(TEntity entity, Guid? parentId)
        {
            //Should find children before Code change
            var children = await FindChildrenAsync(entity, true);

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
        public virtual async Task CopyAsync(Guid id, Guid? parentId)
        {
            var entity = await Repository.GetAsync(id, false, CancellationToken);
            if (entity.ParentId == parentId)
            {
                return;
            }

            await CopyAsync(entity, parentId);
        }

        [UnitOfWork]
        public virtual async Task CopyAsync(TEntity entity, Guid? parentId)
        {
            var newEntity = await CloneAsync(entity);
            newEntity.MoveTo(parentId);
            await CreateAsync(newEntity);

            //要递归复制子节点
            await CopyChildesAsync(entity, newEntity);
        }


        [UnitOfWork]
        public virtual async Task CopyChildesAsync(TEntity source, TEntity target)
        {
            var children = await FindChildrenAsync(source);
            foreach (var child in children)
            {
                var newChild = await CloneAsync(child);
                newChild.MoveTo(target.Id);
                await CreateAsync(newChild);
                await CopyChildesAsync(child, newChild);
            }
        }

        [UnitOfWork]
        public virtual Task<TEntity> CloneAsync(TEntity source)
        {
            throw new NotImplementedException();
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
            var children =
                await Repository.GetListAsync(m => m.ParentId == parentId, cancellationToken: CancellationToken);
            return children.MaxBy(c => c.Code);
        }

        public virtual Task ValidateAsync(TEntity entity)
        {
            return Task.CompletedTask;
        }

        [UnitOfWork]
        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, cancellationToken: CancellationToken);
            return entity.Code;
        }

        [UnitOfWork]
        public virtual async Task<List<TEntity>> FindChildrenAsync(TEntity entity, bool recursive = false)
        {
            if (!recursive)
            {
                return await Repository.GetListAsync(m => m.ParentId == entity.Id, false, CancellationToken);
            }


            return await Repository.GetListAsync(m => m.Code.StartsWith(entity.Code + "."), false, CancellationToken);
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