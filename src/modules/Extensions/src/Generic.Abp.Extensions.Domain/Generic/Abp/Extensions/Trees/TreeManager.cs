using Generic.Abp.Extensions.Exceptions;
using Microsoft.Extensions.Logging;
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
using Volo.Abp.Uow;

namespace Generic.Abp.Extensions.Trees
{
    public abstract class TreeManager<TEntity, TRepository>(
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

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            return await Repository.GetAsync(id, false, CancellationToken);
        }

        public virtual async Task CreateAsync(TEntity entity, bool autoSave = true)
        {
            entity.SetCode(await GetNextChildCodeAsync(entity.ParentId));
            await ValidateAsync(entity);
            await Repository.InsertAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true)
        {
            await ValidateAsync(entity);
            await Repository.UpdateAsync(entity, autoSave, cancellationToken: CancellationToken);
        }

        #region Delete Node

        public virtual async Task DeleteAsync(Guid id, bool autoSave = true)
        {
            var entity = await Repository.GetAsync(id, false, CancellationToken);
            await DeleteAsync(entity, autoSave);
        }

        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = true)
        {
            //await DeleteChildrenAsync(entity, autoSave);
            await Repository.DeleteAllChildrenByCodeAsync(entity.Code, CancellationToken);
            await Repository.DeleteAsync(entity, true, CancellationToken);
        }

        public virtual async Task DeleteChildrenAsync(TEntity entity, bool autoSave = true)
        {
            var children = await Repository.GetListAsync(m => m.ParentId == entity.Id, false, CancellationToken);
            foreach (var child in children)
            {
                await DeleteChildrenAsync(child, autoSave);
                await DeleteAsync(child, autoSave);
            }
        }

        #endregion

        #region Move Node

        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var entity = await Repository.GetAsync(id, false, CancellationToken);
            if (entity.ParentId == parentId)
            {
                return;
            }

            //Store old code of entity
            await MoveAsync(entity, parentId);
        }


        [UnitOfWork]
        public virtual async Task MoveAsync(TEntity entity, Guid? parentId)
        {
            //Should find children before Code change
            // var children = await FindChildrenAsync(entity, false, true);

            //Store old code of entity
            var oldCode = entity.Code;
            var newCode = await GetNextChildCodeAsync(parentId);
            entity.SetCode(newCode);
            await UpdateAsync(entity);
            await Repository.MoveByCodeAsync(oldCode, newCode, CancellationToken);

            //Move
            // entity.SetCode(await GetNextChildCodeAsync(parentId));
            // entity.MoveTo(parentId);
            //
            // await ValidateAsync(entity);
            //
            // //Update Children Codes
            // foreach (var child in children)
            // {
            //     child.SetCode(TreeCodeGenerator.Append(entity.Code,
            //         TreeCodeGenerator.GetRelative(child.Code, oldCode)));
            // }
        }

        #endregion

        #region Copy Node

        public virtual async Task CopyAsync(Guid id, Guid? parentId)
        {
            var entity = await Repository.GetAsync(id, true, CancellationToken);
            if (entity.ParentId == parentId)
            {
                return;
            }

            await CopyAsync(entity, parentId);
        }

        public virtual async Task CopyAsync(TEntity entity, Guid? parentId)
        {
            var newEntity = await CloneAsync(entity);
            newEntity.MoveTo(parentId);
            await CreateAsync(newEntity);

            //复制子节点
            var children = await Repository.GetAllChildrenByCodeAsync(entity.Code, CancellationToken);
            var adds = new List<TEntity>();

            //要递归复制子节点
            await CopyChildrenAsync(entity, newEntity, children, adds);
            // 一次性插入所有节点
            if (adds.Any())
            {
                await Repository.InsertManyAsync(adds, true, CancellationToken);
            }

            await AfterCopyAsync(entity, newEntity);
        }


        public virtual async Task CopyChildrenAsync(TEntity source, TEntity target, List<TEntity> allChildren,
            List<TEntity> adds)
        {
            var queue = new Queue<(TEntity Source, TEntity Target)>();
            queue.Enqueue((source, target));

            while (queue.Any())
            {
                var (currentSource, currentTarget) = queue.Dequeue();
                Logger.LogDebug("Copy Children `{0}`: {1} -> {2}", currentSource.Name, currentSource.Code,
                    currentTarget.Code);
                // 获取当前节点的直接子节点
                var children = allChildren.Where(m => m.ParentId == currentSource.Id).ToList();

                foreach (var oldChild in children)
                {
                    // 克隆并设置父节点信息
                    var newChild = await CloneAsync(oldChild);
                    newChild.MoveTo(currentTarget.Id);

                    // 替换 Code
                    newChild.SetCode(currentTarget.Code + oldChild.Code[(currentSource.Code.Length + 1)..]);
                    Logger.LogDebug("New Children `{0}`: {1} -> {2}", oldChild.Name, oldChild.Code, newChild.Code);

                    // 加入新增列表和队列
                    adds.Add(newChild);
                    queue.Enqueue((oldChild, newChild));
                }
            }
            // var children = allChildren.Where(m => m.ParentId == source.Id).ToList();
            // if (!children.Any())
            // {
            //     return;
            // }
            //
            // foreach (var old in children)
            // {
            //     var newChild = await CloneAsync(old);
            //     newChild.MoveTo(target.Id);
            //     newChild.SetCode(target.Code + source.Code[(source.Code.Length + 1)..]);
            //     adds.Add(newChild);
            //     await CopyChildrenAsync(old, newChild, allChildren, adds);
            // }
        }


        protected virtual Task<TEntity> CloneAsync(TEntity source)
        {
            throw new NotImplementedException();
        }

        protected virtual Task AfterCopyAsync(TEntity entity, TEntity newEntity)
        {
            //TODO: 复制后需要做的其他操作
            return Task.CompletedTask;
        }

        #endregion

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

        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var entity = await Repository.GetAsync(id, cancellationToken: CancellationToken);
            return entity.Code;
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

        public virtual async Task IsAllowMoveOrCopyAsync(TEntity entity, Guid? parentId)
        {
            if (parentId.HasValue && !await Repository.AnyAsync(m => m.Id == parentId, CancellationToken))
            {
                throw new EntityNotFoundBusinessException(nameof(TEntity), parentId);
            }

            if (entity.Id == parentId || entity.ParentId == parentId)
            {
                throw new CannotMoveOrCopyToItselfBusinessException();
            }
        }
    }
}