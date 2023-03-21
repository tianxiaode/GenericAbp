using System.Collections.Generic;

namespace Generic.Abp.OpenIddict.WildcardDomains;

public class AbpOpenIddictWildcardDomainOptions
{
    public bool EnableWildcardDomainSupport { get; set; }

    public HashSet<string> WildcardDomainsFormat { get; }

    public AbpOpenIddictWildcardDomainOptions()
    {
        WildcardDomainsFormat = new HashSet<string>();
    }
}