using System.Threading.Tasks;
using System;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.FileManagement.DataSeeds;

public interface IResourceDataSeed
{
    Task SeedAsync(Guid? tenantId = null);
}