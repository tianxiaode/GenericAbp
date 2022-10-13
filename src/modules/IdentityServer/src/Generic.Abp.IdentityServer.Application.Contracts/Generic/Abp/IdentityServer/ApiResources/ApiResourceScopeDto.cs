using System;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceScopeDto
{
    public string Scope { get; set; }

    public ApiResourceScopeDto(string scope)
    {
        Scope = scope;
    }
}