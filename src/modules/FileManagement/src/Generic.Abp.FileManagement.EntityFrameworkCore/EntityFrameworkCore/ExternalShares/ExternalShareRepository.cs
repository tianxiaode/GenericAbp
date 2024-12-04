using Generic.Abp.Extensions.Extensions;
using Generic.Abp.FileManagement.ExternalShares;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Abp.Extensions.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.ExternalShares;

public class ExternalShareRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : ExtensionRepository<IFileManagementDbContext, ExternalShare>(
            dbContextProvider),
        IExternalShareRepository
{
}