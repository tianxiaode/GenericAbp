using System.Threading.Tasks;

namespace Generic.Abp.TailWindCss.Account.Web.OpenIddict.ClaimDestinations;

public interface IAbpOpenIddictClaimDestinationsProvider
{
    Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context);
}