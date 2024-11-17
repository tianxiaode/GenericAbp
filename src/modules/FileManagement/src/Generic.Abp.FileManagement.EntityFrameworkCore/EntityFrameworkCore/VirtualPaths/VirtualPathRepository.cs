using System;
using Generic.Abp.FileManagement.VirtualPaths;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;

public class VirtualPathRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : EfCoreRepository<FileManagementDbContext, VirtualPath, Guid>(dbContextProvider), IVirtualPathRepository
{
}