using Generic.Abp.ExternalAuthentication.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Generic.Abp.ExternalAuthentication.Settings;

public class ExternalAuthenticationSettingDefinitionProvider : SettingDefinitionProvider
{
    protected readonly IAuthenticationSchemeProvider SchemeProvider;

    public ExternalAuthenticationSettingDefinitionProvider(IAuthenticationSchemeProvider schemeProvider)
    {
        SchemeProvider = schemeProvider;
    }

    public override void Define(ISettingDefinitionContext context)
    {
        var schemes = GetAuthenticationSchemesAsync().Result;
        foreach (var scheme in schemes)
        {
            context.Add(new SettingDefinition(
                $"{ExternalAuthenticationSettingNames.Provider.ProviderPrefix}{scheme.Name}", "",
                L(scheme.DisplayName ?? scheme.Name), isEncrypted: true));
        }

        context.Add(new SettingDefinition(ExternalAuthenticationSettingNames.NewUser.NewUserPrefix, "",
            L("NewUserPrefix")));
        context.Add(new SettingDefinition(ExternalAuthenticationSettingNames.NewUser.NewUserEmailSuffix, "",
            L("NewUserEmailSuffix")));
    }

    private async Task<IEnumerable<AuthenticationScheme>> GetAuthenticationSchemesAsync()
    {
        var schemes = await SchemeProvider.GetAllSchemesAsync();
        return schemes;
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExternalAuthenticationResource>(name);
    }
}