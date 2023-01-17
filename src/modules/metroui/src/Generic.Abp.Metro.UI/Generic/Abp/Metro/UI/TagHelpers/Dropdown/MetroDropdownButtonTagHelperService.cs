using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.TagHelpers.Button;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers.Dropdown;

public class MetroDropdownButtonTagHelperService : MetroTagHelperService<MetroDropdownButtonTagHelper>
{

    private readonly HtmlEncoder _htmlEncoder;
    private readonly IServiceProvider _serviceProvider;

    public MetroDropdownButtonTagHelperService(
        HtmlEncoder htmlEncoder,
        IServiceProvider serviceProvider)
    {
        _htmlEncoder = htmlEncoder;
        _serviceProvider = serviceProvider;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var content = await output.GetChildContentAsync();

        var buttonsAsHtml = await GetButtonsAsHtmlAsync(context, output, content);

        output.PreElement.SetHtmlContent(buttonsAsHtml);

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Content.SetContent("");
        output.Attributes.Clear();
        AddClasses(context, output);
    }

    protected virtual async Task<string> GetButtonsAsHtmlAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperContent content)
    {
        var buttonBuilder = new StringBuilder("");

        var mainButton = await GetMainButtonAsync(context, output, content);

        buttonBuilder.AppendLine(mainButton);

        if (TagHelper.DropdownStyle == DropdownStyle.Split)
        {
            var splitButton = await GetSplitButtonAsync(context, output);

            buttonBuilder.AppendLine(splitButton);
        }

        return buttonBuilder.ToString();
    }

    protected virtual async Task<string> GetMainButtonAsync(TagHelperContext context, TagHelperOutput output, TagHelperContent content)
    {
        var abpButtonTagHelper = _serviceProvider.GetRequiredService<MetroButtonTagHelper>();

        abpButtonTagHelper.Icon = TagHelper.Icon;
        abpButtonTagHelper.Text = TagHelper.Text;
        abpButtonTagHelper.IconType = TagHelper.IconType;
        abpButtonTagHelper.Size = TagHelper.Size;
        abpButtonTagHelper.ButtonType = TagHelper.ButtonType;
        abpButtonTagHelper.Cls = TagHelper.Cls;
        var attributes = GetAttributesForMainButton(context, output);

        var buttonTag = await abpButtonTagHelper.ProcessAndGetOutputAsync(attributes, context, "button", TagMode.StartTagAndEndTag);

        buttonTag.PreContent.SetHtmlContent(content);

        if ((!(TagHelper.NavLink ?? false)) && (!(TagHelper.Link ?? false))) return buttonTag.Render(_htmlEncoder);
        var linkTag = ConvertButtonToLink(buttonTag);
        return linkTag.Render(_htmlEncoder);

    }

    protected virtual async Task<string> GetSplitButtonAsync(TagHelperContext context, TagHelperOutput output)
    {
        var abpButtonTagHelper = _serviceProvider.GetRequiredService<MetroButtonTagHelper>();

        abpButtonTagHelper.Size = TagHelper.Size;
        abpButtonTagHelper.ButtonType = TagHelper.ButtonType;
        var attributes = GetAttributesForSplitButton(context, output);

        return await abpButtonTagHelper.RenderAsync(attributes, context, _htmlEncoder, "button", TagMode.StartTagAndEndTag);
    }

    protected virtual TagHelperAttributeList GetAttributesForMainButton(TagHelperContext context, TagHelperOutput output)
    {

        var attributes = new TagHelperAttributeList();

        foreach (var tagHelperAttribute in output.Attributes)
        {
            attributes.Add(tagHelperAttribute);
        }

        if (TagHelper.DropdownStyle == DropdownStyle.Split) return attributes;
        attributes.AddClass("dropdown-toggle");

        return attributes;
    }

    protected virtual TagHelperAttributeList GetAttributesForSplitButton(TagHelperContext context, TagHelperOutput output)
    {
        var attributes = new TagHelperAttributeList();

        attributes.AddClass("dropdown-toggle");
        attributes.AddClass("split");

        return attributes;
    }

    protected virtual TagHelperOutput ConvertButtonToLink(TagHelperOutput buttonTag)
    {
        buttonTag.TagName = "a";
        buttonTag.Attributes.RemoveAll("type");
        buttonTag.Attributes.Add("roles", "button");
        buttonTag.Attributes.Add("href", "#");

        if (TagHelper.NavLink ?? false)
        {
            buttonTag.Attributes.AddClass("nav-link");
        }
        return buttonTag;
    }
}
