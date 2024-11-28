using Generic.Abp.Extensions.Trees;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : ITreeRepository<Resource>
{
    Task<Expression<Func<Resource, bool>>> BuildQueryExpressionAsync(
        Guid parentId,
        string? filter,
        DateTime? startTime,
        DateTime? endTime,
        string? filetype);
}