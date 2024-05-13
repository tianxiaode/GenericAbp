using Microsoft.Extensions.Options;
using OpenIddict.Server;
using Volo.Abp;

namespace Generic.Abp.Tailwind.OpenIddict.WildcardDomains;

public class AbpValidatePostLogoutRedirectUriParameter : AbpOpenIddictWildcardDomainBase<
    OpenIddictServerHandlers.Session.ValidatePostLogoutRedirectUriParameter,
    OpenIddictServerEvents.ValidateLogoutRequestContext>
{
    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ValidateLogoutRequestContext>()
            .UseSingletonHandler<AbpValidatePostLogoutRedirectUriParameter>()
            .SetOrder(int.MinValue + 100_000)
            .SetType(OpenIddictServerHandlerType.BuiltIn)
            .Build();

    public AbpValidatePostLogoutRedirectUriParameter(
        IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainsOptions)
        : base(wildcardDomainsOptions)
    {
    }

    public override async ValueTask HandleAsync(OpenIddictServerEvents.ValidateLogoutRequestContext context)
    {
        Check.NotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.PostLogoutRedirectUri) ||
            await CheckWildcardDomainAsync(context.PostLogoutRedirectUri))
        {
            return;
        }

        await Handler!.HandleAsync(context);
    }
}