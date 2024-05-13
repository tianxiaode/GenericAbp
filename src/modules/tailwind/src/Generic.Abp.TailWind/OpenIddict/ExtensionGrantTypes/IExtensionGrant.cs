using Microsoft.AspNetCore.Mvc;

namespace Generic.Abp.Tailwind.OpenIddict.ExtensionGrantTypes;

public interface IExtensionGrant
{
    string Name { get; }

    Task<IActionResult> HandleAsync(ExtensionGrantContext context);
}