using Microsoft.Extensions.Options;
using OpenIddict.Server;
using Volo.Abp;

namespace Generic.Abp.Tailwind.OpenIddict.WildcardDomains;

public class AbpValidateRedirectUriParameter : AbpOpenIddictWildcardDomainBase<
    OpenIddictServerHandlers.Authentication.ValidateRedirectUriParameter,
    OpenIddictServerEvents.ValidateAuthorizationRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateAuthorizationRequestContext>()
            .UseSingletonHandler<AbpValidateRedirectUriParameter>()
            .SetOrder(OpenIddictServerHandlers.Authentication.ValidateClientIdParameter.Descriptor.Order + 1_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidateRedirectUriParameter(IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions)
        : base(wildcardDomainsOptions)
    {
    }

    public override async ValueTask HandleAsync(OpenIddictServerEvents.ValidateAuthorizationRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (!string.IsNullOrEmpty(context.RedirectUri) && await CheckWildcardDomainAsync(context.RedirectUri))
        {
            return;
        }

        await Handler!.HandleAsync(context);
    }
}