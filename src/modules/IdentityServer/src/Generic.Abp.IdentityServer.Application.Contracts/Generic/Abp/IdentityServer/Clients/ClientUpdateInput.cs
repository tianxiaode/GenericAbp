using System;
using Volo.Abp.Domain.Entities;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientUpdateInput: ClientCreateOrUpdateInput, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}