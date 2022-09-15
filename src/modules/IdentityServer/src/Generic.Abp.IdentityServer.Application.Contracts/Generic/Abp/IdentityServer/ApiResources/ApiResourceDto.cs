using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceDto: AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public string AllowedAccessTokenSigningAlgorithms { get; set; }
    public bool ShowInDiscoveryDocument { get; set; } = true;
    public string ConcurrencyStamp { get; set; }
}