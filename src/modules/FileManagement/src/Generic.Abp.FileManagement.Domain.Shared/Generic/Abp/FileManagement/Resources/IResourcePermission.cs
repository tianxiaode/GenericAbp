using System;
using Volo.Abp.MultiTenancy;

namespace Generic.Abp.FileManagement.Resources;

public interface IResourcePermission : IMultiTenant
{
    Guid ResourceId { get; }
    string ProviderName { get; }
    string? ProviderKey { get; }
    int Permissions { get; }
}