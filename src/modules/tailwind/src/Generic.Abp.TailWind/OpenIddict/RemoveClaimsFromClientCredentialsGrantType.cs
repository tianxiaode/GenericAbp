﻿using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Generic.Abp.Tailwind.OpenIddict;

public class
    RemoveClaimsFromClientCredentialsGrantType : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessSignInContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessSignInContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireAccessTokenGenerated>()
            .UseSingletonHandler<RemoveClaimsFromClientCredentialsGrantType>()
            .SetOrder(OpenIddictServerHandlers.PrepareAccessTokenPrincipal.Descriptor.Order - 1)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ValueTask HandleAsync(OpenIddictServerEvents.ProcessSignInContext context)
    {
        if (!context.Request.IsClientCredentialsGrantType())
        {
            return default;
        }

        if (context.Principal == null)
        {
            return default;
        }

        context.Principal.RemoveClaims(OpenIddictConstants.Claims.Subject);
        context.Principal.RemoveClaims(OpenIddictConstants.Claims.PreferredUsername);

        return default;
    }
}