using Generic.Abp.Extensions.EntityFrameworkCore;
using Generic.Abp.FileManagement.VirtualPaths;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;

public class VirtualPathRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : ExtensionRepository<FileManagementDbContext, VirtualPath>(dbContextProvider),
        IVirtualPathRepository
{
}