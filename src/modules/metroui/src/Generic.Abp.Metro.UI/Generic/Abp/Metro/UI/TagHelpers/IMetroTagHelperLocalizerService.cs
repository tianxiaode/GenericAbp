using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers;

public interface IMetroTagHelperLocalizerService : ITransientDependency
{
    Task<string> GetLocalizedTextAsync(string text, ModelExplorer explorer);

    Task<IStringLocalizer> GetLocalizerOrNullAsync(ModelExplorer explorer);

    Task<IStringLocalizer> GetLocalizerOrNullAsync(Assembly assembly);
}