using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Localization;

namespace Generic.Abp.Metro.UI.TagHelpers;

public class MetroTagHelperLocalizer : IMetroTagHelperLocalizer
{
    private readonly IStringLocalizerFactory _stringLocalizerFactory;
    private readonly AbpMvcDataAnnotationsLocalizationOptions _options;

    public MetroTagHelperLocalizer(IOptions<AbpMvcDataAnnotationsLocalizationOptions> options, IStringLocalizerFactory stringLocalizerFactory)
    {
        _stringLocalizerFactory = stringLocalizerFactory;
        _options = options.Value;
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
            ? _stringLocalizerFactory.CreateDefaultOrNull()
            : _stringLocalizerFactory.Create(resourceType);
    }

    private Task<Type> GetResourceTypeAsync(Assembly assembly)
    {
        return Task.FromResult(_options
            .AssemblyResources
            .GetOrDefault(assembly));
    }
}
