using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Generic.Abp.Metro.UI.TagHelpers.NavigationView;

public class MetroNavigationPanelTagHelper : MetroTagHelper
{
    public bool? PullButton { get; set; }
    public bool? SuggestBox { get; set; }

    private const string SuggestBoxTemplate = @"
        <div class=""suggest-box"">
            <input type=""text"" data-role=""input"" data-clear-button=""false"" data-search-button=""true"">
            <button class=""holder"">
                <span class=""mif-search""></span>
            </button>
        </div>        
        ";

    private const string PullButtonTemplate = @"
        <button class=""pull-button"">
            <span class=""default-icon-menu""></span>
        </button>
    ";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        var attributes = output.Attributes;
        attributes.AddClass("navview-pane");
        var child = await output.GetChildContentAsync();
        await AddPullButtonAsync(context, output);
        await AddSuggestBoxAsync(context, output);
        output.Content.AppendHtml(child);
    }

    protected virtual Task AddPullButtonAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (PullButton != true) return Task.CompletedTask;

        output.Content.AppendHtml(PullButtonTemplate);
        return Task.CompletedTask;
    }

    protected virtual Task AddSuggestBoxAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (SuggestBox != true) return Task.CompletedTask;

        output.Content.AppendHtml(SuggestBoxTemplate);
        return Task.CompletedTask;
    }
}