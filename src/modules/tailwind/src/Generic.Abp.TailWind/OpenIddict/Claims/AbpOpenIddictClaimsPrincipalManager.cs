﻿using System.Security.Claims;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Tailwind.OpenIddict.Claims;

public class AbpOpenIddictClaimsPrincipalManager : ISingletonDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IOptions<AbpOpenIddictClaimsPrincipalOptions> Options { get; }

    public AbpOpenIddictClaimsPrincipalManager(IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpOpenIddictClaimsPrincipalOptions> options)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Options = options;
    }

    public virtual async Task HandleAsync(OpenIddictRequest openIddictRequest, ClaimsPrincipal principal)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var providerType in Options.Value.ClaimsPrincipalHandlers)
            {
                var provider =
                    (IAbpOpenIddictClaimsPrincipalHandler)scope.ServiceProvider.GetRequiredService(providerType);
                await provider.HandleAsync(
                    new AbpOpenIddictClaimsPrincipalHandlerContext(scope.ServiceProvider, openIddictRequest,
                        principal));
            }
        }
    }
}