using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Generic.Abp.Tailwind.OpenIddict.Claims;

public class AbpDynamicClaimsOpenIddictClaimsPrincipalHandler : IAbpOpenIddictClaimsPrincipalHandler,
    ITransientDependency
{
    public virtual async Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context)
    {
        var abpClaimsPrincipalFactory = context
            .ScopeServiceProvider
            .GetRequiredService<IAbpClaimsPrincipalFactory>();

        await abpClaimsPrincipalFactory.CreateDynamicAsync(context.Principal);
    }
}