using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.MenuManagement;

public interface IMenuDataSeed : ITransientDependency
{
    Task SeedAsync(Guid? tenantId = null);
}