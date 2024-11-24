using System.Threading.Tasks;
using System;

namespace Generic.Abp.FileManagement.DataSeeds;

public interface IResourceDataSeed
{
    Task SeedAsync(Guid? tenantId = null);
}