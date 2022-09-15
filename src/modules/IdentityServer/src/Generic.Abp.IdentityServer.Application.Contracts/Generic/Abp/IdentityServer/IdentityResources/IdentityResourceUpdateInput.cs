using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.IdentityServer.IdentityResources;

[Serializable]
public class IdentityResourceUpdateInput: IdentityResourceCreateOrUpdateInput, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}