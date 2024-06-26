﻿using Microsoft.Extensions.Options;
using OpenIddict.Server;
using Volo.Abp.Text.Formatting;

namespace Generic.Abp.Tailwind.OpenIddict.WildcardDomains;

public abstract class AbpOpenIddictWildcardDomainBase<THandler, TContext> : IOpenIddictServerHandler<TContext>
    where THandler : class
    where TContext : OpenIddictServerEvents.BaseContext
{
    protected THandler Handler { get; set; }
    protected AbpOpenIddictWildcardDomainOptions WildcardDomainOptions { get; }

    protected AbpOpenIddictWildcardDomainBase(IOptions<AbpOpenIddictWildcardDomainOptions> wildcardDomainOptions,
        THandler handler)
    {
        WildcardDomainOptions = wildcardDomainOptions.Value;
        Handler = handler;
    }

    public abstract ValueTask HandleAsync(TContext context);

    protected virtual Task<bool> CheckWildcardDomainAsync(string url)
    {
        foreach (var domainFormat in WildcardDomainOptions.WildcardDomainsFormat)
        {
            var extractResult = FormattedStringValueExtracter.Extract(url, domainFormat, ignoreCase: true);
            if (extractResult.IsMatch)
            {
                return Task.FromResult(true);
            }
        }

        foreach (var domainFormat in WildcardDomainOptions.WildcardDomainsFormat)
        {
            if (domainFormat.Replace("{0}.", "").Equals(url, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}