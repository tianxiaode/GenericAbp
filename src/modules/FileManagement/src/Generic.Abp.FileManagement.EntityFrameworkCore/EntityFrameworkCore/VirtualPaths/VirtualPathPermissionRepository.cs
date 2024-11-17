using System;
using Generic.Abp.FileManagement.VirtualPaths;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.VirtualPaths;

public class VirtualPathPermissionRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : EfCoreRepository<FileManagementDbContext, VirtualPathPermission, Guid>(dbContextProvider),
        IVirtualPathPermissionRepository;