using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers;

public class MetroTagHelperLocalizerService : IMetroTagHelperLocalizerService
{
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected AbpMvcDataAnnotationsLocalizationOptions Options { get; }

    public MetroTagHelperLocalizerService(IOptions<AbpMvcDataAnnotationsLocalizationOptions> options,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        StringLocalizerFactory = stringLocalizerFactory;
        Options = options.Value;
    }

    public async Task<string> GetLocalizedTextAsync(string text, ModelExplorer explorer)
    {
        var localizer = await GetLocalizerOrNullAsync(explorer);
        return localizer == null
            ? text
            : localizer[text].Value;
    }

    public async Task<IStringLocalizer> GetLocalizerOrNullAsync(ModelExplorer explorer)
    {
        return await GetLocalizerOrNullAsync(explorer.Container.ModelType.Assembly);
    }

    public async Task<IStringLocalizer> GetLocalizerOrNullAsync(Assembly assembly)
    {
        var resourceType = await GetResourceTypeAsync(assembly);
        return resourceType == null
            ? StringLocalizerFactory.CreateDefaultOrNull()
            : StringLocalizerFactory.Create(resourceType);
    }

    private Task<Type> GetResourceTypeAsync(Assembly assembly)
    {
        return Task.FromResult(Options
            .AssemblyResources
            .GetOrDefault(assembly));
    }
}