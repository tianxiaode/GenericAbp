using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.IdentityServer.ApiScopes;

[Serializable]
public class ApiScopeDto: AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public virtual string Name { get; set; }

    public virtual string DisplayName { get; set; }

    public virtual string Description { get; set; }

    public virtual bool Enabled { get; set; }

    public virtual bool Required { get; set; }

    public virtual bool Emphasize { get; set; }

    public virtual bool ShowInDiscoveryDocument { get; set; }
    public string ConcurrencyStamp { get; set; }
}