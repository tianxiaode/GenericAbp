using Generic.Abp.Extensions.EntityFrameworkCore;
using Generic.Abp.FileManagement.ExternalShares;
using Volo.Abp.EntityFrameworkCore;

namespace Generic.Abp.FileManagement.EntityFrameworkCore.ExternalShares;

public class ExternalShareRepository(IDbContextProvider<IFileManagementDbContext> dbContextProvider)
    : ExtensionRepository<IFileManagementDbContext, ExternalShare>(
            dbContextProvider),
        IExternalShareRepository
{
}