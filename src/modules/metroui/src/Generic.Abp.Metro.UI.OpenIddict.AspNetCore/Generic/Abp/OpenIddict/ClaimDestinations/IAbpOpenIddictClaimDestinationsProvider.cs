using System.Threading.Tasks;

namespace Generic.Abp.OpenIddict.ClaimDestinations;

public interface IAbpOpenIddictClaimDestinationsProvider
{
    Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context);
}