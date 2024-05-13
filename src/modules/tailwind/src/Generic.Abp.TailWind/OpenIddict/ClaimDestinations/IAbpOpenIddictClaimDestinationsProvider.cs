namespace Generic.Abp.Tailwind.OpenIddict.ClaimDestinations;

public interface IAbpOpenIddictClaimDestinationsProvider
{
    Task SetDestinationsAsync(AbpOpenIddictClaimDestinationsProviderContext context);
}