using System.Reflection;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public interface IAbpTagHelperLocalizer : ITransientDependency
{
    string GetLocalizedText(string text, ModelExplorer explorer);

    IStringLocalizer GetLocalizerOrNull(ModelExplorer explorer);

    IStringLocalizer GetLocalizerOrNull(Assembly assembly);
}
