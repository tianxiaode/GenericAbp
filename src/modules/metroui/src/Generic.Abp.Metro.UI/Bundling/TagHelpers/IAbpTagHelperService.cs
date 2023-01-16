using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public interface IAbpTagHelperService<TTagHelper> : ITransientDependency
    where TTagHelper : TagHelper
{
    TTagHelper TagHelper { get; }

    int Order { get; }

    void Init(TagHelperContext context);

    void Process(TagHelperContext context, TagHelperOutput output);

    Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
}
