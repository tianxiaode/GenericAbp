using System.Threading.Tasks;
using System;
using System.Linq;

namespace Generic.Abp.Extensions.Trees;

public abstract partial class TreeManager<TEntity, TRepository>
{
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

        return TreeCodeGenerator.Append(parentCode ?? "", TreeCodeGenerator.Create(1));
    }

    public virtual async Task<TEntity?> GetLastChildOrNullAsync(Guid? parentId)
    {
        var children = await Repository.GetListAsync(m => m.ParentId == parentId, cancellationToken: CancellationToken);
        return children.MaxBy(c => c.Code);
    }

    public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
    {
        var entity = await Repository.GetAsync(id, cancellationToken: CancellationToken);
        return entity.Code;
    }
}