using System;
using Volo.Abp.Domain.Repositories;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourceConfigurationRepository : IRepository<ResourceConfiguration, Guid>
{
}