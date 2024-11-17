using System;
using Generic.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Files;

public class FilePermissionRepository(IDbContextProvider<FileManagementDbContext> dbContextProvider)
    : EfCoreRepository<FileManagementDbContext, FilePermission, Guid>(dbContextProvider), IFilePermissionRepository;