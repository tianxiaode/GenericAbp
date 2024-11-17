using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generic.Abp.FileManagement.Files;
using Generic.Abp.FileManagement.Folders;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.Folders;

public class FolderRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : EfCoreRepository<IFileManagementDbContext, Folder, Guid>(dbContextProvider), IFolderRepository
{
}