using Generic.Abp.Extensions.Trees;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceRepository : ITreeRepository<Resource>
{
    Task<Resource?> GetInheritedPermissionParentAsync(Guid id, string code, CancellationToken cancellationToken);
    Task<long> SumSizeByCodeAsync(string code);
}