using System.Threading.Tasks;
using System;

namespace Generic.Abp.FileManagement.DataSeeds;

public interface IFolderDataSeed
{
    Task SeedAsync(Guid? tenantId = null);
}