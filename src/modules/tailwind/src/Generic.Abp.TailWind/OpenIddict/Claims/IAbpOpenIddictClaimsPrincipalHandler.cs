namespace Generic.Abp.Tailwind.OpenIddict.Claims;

public interface IAbpOpenIddictClaimsPrincipalHandler
{
    Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context);
}