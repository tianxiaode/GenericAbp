using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.IdentityServer.ApiScopes;

[Serializable]
public class ApiScopeUpdateInput: ApiScopeCreateOrUpdateInput, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}