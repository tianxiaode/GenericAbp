using OpenIddict.Abstractions;
using System.Linq.Dynamic.Core;
using System.Web;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Tailwind.OpenIddict;

public class AbpOpenIddictRequestHelper : ITransientDependency
{
    public virtual Task<OpenIddictRequest?> GetFromReturnUrlAsync(string returnUrl)
    {
        if (returnUrl.IsNullOrWhiteSpace())
        {
            return Task.FromResult<OpenIddictRequest?>(null);
        }

        var qm = returnUrl.IndexOf("?", StringComparison.Ordinal);
        if (qm <= 0)
        {
            return Task.FromResult<OpenIddictRequest?>(null);
        }

        //提取url参数
        var query = HttpUtility.ParseQueryString(returnUrl);
        if (query.Count == 0)
        {
            return Task.FromResult<OpenIddictRequest?>(null);
        }

        var pa =
            (from string? s in query where !s.IsNullOrEmpty() select new KeyValuePair<string, string?>(s, query[s]))
            .ToList();
        return Task.FromResult<OpenIddictRequest?>(new OpenIddictRequest(pa));
    }
}