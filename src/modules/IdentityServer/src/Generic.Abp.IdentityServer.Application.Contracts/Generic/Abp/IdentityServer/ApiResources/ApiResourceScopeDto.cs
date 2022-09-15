using System;

namespace Generic.Abp.IdentityServer.ApiResources;

[Serializable]
public class ApiResourceScopeDto
{
    public string ScopeName { get; set; }
    public string ScopeDisplayName { get; set; }
    public bool IsSelected { get; set; }

    public ApiResourceScopeDto(string scopeName, string scopeDisplayName)
    {
        ScopeName = scopeName;
        ScopeDisplayName = scopeDisplayName;
        IsSelected = false;
    }
}