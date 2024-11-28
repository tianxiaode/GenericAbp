using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Resources;

public partial class ResourceManager
{
    public virtual async Task<bool> FileExistsAsync(string code, Guid fileInfoBaseId)
    {
        return await Repository.AnyAsync(m => m.Code.StartsWith(code + ".") && m.FileInfoBaseId == fileInfoBaseId);
    }
}