using System.Security.Claims;

namespace Generic.Abp.Tailwind.OpenIddict.ClaimDestinations;

public class AbpOpenIddictClaimDestinationsProviderContext
{
    public IServiceProvider ScopeServiceProvider { get; }

    public ClaimsPrincipal Principal { get; }

    public Claim[] Claims { get; }

    public AbpOpenIddictClaimDestinationsProviderContext(IServiceProvider scopeServiceProvider,
        ClaimsPrincipal principal, Claim[] claims)
    {
        ScopeServiceProvider = scopeServiceProvider;
        Principal = principal;
        Claims = claims;
    }
}