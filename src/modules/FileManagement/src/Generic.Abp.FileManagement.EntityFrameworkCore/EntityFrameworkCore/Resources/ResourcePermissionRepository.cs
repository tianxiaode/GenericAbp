using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Resources;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Resources;

public class ResourcePermissionRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, ResourcePermission, Guid>(dbContextProvider),
        IResourcePermissionRepository;