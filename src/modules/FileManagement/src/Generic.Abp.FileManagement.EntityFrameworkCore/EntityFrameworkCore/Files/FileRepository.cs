using System;
using Generic.Abp.FileManagement.Files;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Files;

public class FileRepository : EfCoreRepository<IFileManagementDbContext, File, Guid>, IFileRepository
{
    public FileRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}