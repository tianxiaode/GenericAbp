using System;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Folders;

public class FolderPermissionRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, FolderPermission, Guid>(dbContextProvider),
        IFolderPermissionRepository;