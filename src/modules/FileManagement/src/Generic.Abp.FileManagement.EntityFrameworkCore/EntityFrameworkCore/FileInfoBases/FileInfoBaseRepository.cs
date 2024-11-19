using Generic.Abp.FileManagement.FileInfoBases;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.FileInfoBases;

public class FileInfoBaseRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, FileInfoBase, Guid>(dbContextProvider), IFileInfoBaseRepository
{
}