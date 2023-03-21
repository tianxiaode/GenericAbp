using Volo.Abp.Collections;

namespace Generic.Abp.OpenIddict.ClaimDestinations;

public class AbpOpenIddictClaimDestinationsOptions
{
    public ITypeList<IAbpOpenIddictClaimDestinationsProvider> ClaimDestinationsProvider { get; }

    public AbpOpenIddictClaimDestinationsOptions()
    {
        ClaimDestinationsProvider = new TypeList<IAbpOpenIddictClaimDestinationsProvider>();
    }
}