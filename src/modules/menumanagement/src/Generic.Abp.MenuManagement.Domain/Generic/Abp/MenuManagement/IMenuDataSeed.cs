using System;
using System.Threading.Tasks;

namespace Generic.Abp.MenuManagement;

public interface IMenuDataSeed
{
    Task SeedAsync(Guid? tenantId = null);
}