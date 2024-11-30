using Generic.Abp.FileManagement.Resources;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Resources;

public class ResourcePermissionRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, ResourcePermission, Guid>(dbContextProvider),
        IResourcePermissionRepository;