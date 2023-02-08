using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

[HtmlTargetElement(Attributes = "metro-id-Name")]
public class MetroIdNameTagHelper : MetroTagHelper
{
    /// <summary>
    /// Make sure this TagHelper is executed first.
    /// </summary>
    public override int Order => -1000 - 1;

    [HtmlAttributeName("metro-id-Name")] public ModelExpression IdNameFor { get; set; }

    private readonly MvcViewOptions _mvcViewOptions;

    public MetroIdNameTagHelper(IOptions<MvcViewOptions> mvcViewOptions)
    {
        _mvcViewOptions = mvcViewOptions.Value;
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (IdNameFor == null) return Task.CompletedTask;
        if (!context.AllAttributes.Any(x => x.Name.Equals("id", StringComparison.OrdinalIgnoreCase)))
        {
            var id = TagBuilder.CreateSanitizedId(IdNameFor.Name,
                _mvcViewOptions.HtmlHelperOptions.IdAttributeDotReplacement);
            output.Attributes.Add("id", id);
        }

        if (!context.AllAttributes.Any(x => x.Name.Equals("Name", StringComparison.OrdinalIgnoreCase)))
        {
            output.Attributes.Add("Name", IdNameFor.Name);
        }

        return Task.CompletedTask;
    }
}