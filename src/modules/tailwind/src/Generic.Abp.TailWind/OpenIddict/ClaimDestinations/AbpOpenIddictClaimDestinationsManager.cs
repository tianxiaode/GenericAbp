﻿using System.Security.Claims;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Tailwind.OpenIddict.ClaimDestinations;

public class AbpOpenIddictClaimDestinationsManager : ISingletonDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<AbpOpenIddictClaimDestinationsOptions> Options { get; }

    public AbpOpenIddictClaimDestinationsManager(IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpOpenIddictClaimDestinationsOptions> options)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options;
    }

    public virtual async Task SetAsync(ClaimsPrincipal principal)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var providerType in Options.Value.ClaimDestinationsProvider)
            {
                var provider =
                    (IAbpOpenIddictClaimDestinationsProvider)scope.ServiceProvider.GetRequiredService(providerType);
                await provider.SetDestinationsAsync(
                    new AbpOpenIddictClaimDestinationsProviderContext(scope.ServiceProvider, principal,
                        principal.Claims.ToArray()));
            }
        }
    }
}