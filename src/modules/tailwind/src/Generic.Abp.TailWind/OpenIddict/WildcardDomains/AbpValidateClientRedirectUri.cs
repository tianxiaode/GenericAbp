﻿using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp;

namespace Generic.Abp.Tailwind.OpenIddict.WildcardDomains;

public class AbpValidateClientRedirectUri : AbpOpenIddictWildcardDomainBase<
    OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri,
    OpenIddictServerEvents.ValidateAuthorizationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateAuthorizationRequestContext>()
            .AddFilter<OpenIddictServerHandlerFilters.RequireDegradedModeDisabled>()
            .UseScopedHandler<AbpValidateClientRedirectUri>()
            .SetOrder(OpenIddictServerHandlers.Authentication.ValidateResponseType.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateClientRedirectUri(
        IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions,
        IOpenIddictApplicationManager applicationManager)
        : base(wildcardDomainsOptions,
            new OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri(applicationManager))
    {
        Handler = new OpenIddictServerHandlers.Authentication.ValidateClientRedirectUri(applicationManager);
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateAuthorizationRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (!string.IsNullOrEmpty(context.RedirectUri) && await CheckWildcardDomainAsync(context.RedirectUri))
        {
            return;
        }

        await Handler.HandleAsync(context);
    }
}