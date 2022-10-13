using System;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceScopeDeleteInput
{
    public string Scope { get; set; }
}