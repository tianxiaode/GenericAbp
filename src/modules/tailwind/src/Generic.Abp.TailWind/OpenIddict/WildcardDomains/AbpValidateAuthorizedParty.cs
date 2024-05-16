using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp;

namespace Generic.Abp.Tailwind.OpenIddict.WildcardDomains;

public class AbpValidateAuthorizedParty : AbpOpenIddictWildcardDomainBase<
    OpenIddictServerHandlers.Session.ValidateAuthorizedParty, OpenIddictServerEvents.ValidateLogoutRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateLogoutRequestContext>()
            .UseScopedHandler<AbpValidateAuthorizedParty>()
            .SetOrder(OpenIddictServerHandlers.Session.ValidateEndpointPermissions.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateAuthorizedParty(
        IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions,
        IOpenIddictApplicationManager applicationManager)
        : base(wildcardDomainsOptions, new OpenIddictServerHandlers.Session.ValidateAuthorizedParty(applicationManager))
    {
        Handler = new OpenIddictServerHandlers.Session.ValidateAuthorizedParty(applicationManager);
    }

    public async override ValueTask HandleAsync(OpenIddictServerEvents.ValidateLogoutRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (await CheckWildcardDomainAsync(context.PostLogoutRedirectUri))
        {
            return;
        }

        await Handler.HandleAsync(context);
    }
}