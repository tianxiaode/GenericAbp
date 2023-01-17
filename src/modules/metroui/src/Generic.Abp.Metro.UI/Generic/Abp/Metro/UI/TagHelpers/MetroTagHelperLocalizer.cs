using System;
using System.Collections.Generic;
using System.Reflection;
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

    public string GetLocalizedText(string text, ModelExplorer explorer)
    {
        var localizer = GetLocalizerOrNull(explorer);
        return localizer == null
            ? text
            : localizer[text].Value;
    }

    public IStringLocalizer GetLocalizerOrNull(ModelExplorer explorer)
    {
        return GetLocalizerOrNull(explorer.Container.ModelType.Assembly);
    }

    public IStringLocalizer GetLocalizerOrNull(Assembly assembly)
    {
        var resourceType = GetResourceType(assembly);
        return resourceType == null
            ? _stringLocalizerFactory.CreateDefaultOrNull()
            : _stringLocalizerFactory.Create(resourceType);
    }

    private Type GetResourceType(Assembly assembly)
    {
        return _options
            .AssemblyResources
            .GetOrDefault(assembly);
    }
}
