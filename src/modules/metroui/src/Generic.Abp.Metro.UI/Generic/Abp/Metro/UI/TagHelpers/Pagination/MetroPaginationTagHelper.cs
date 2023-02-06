using Generic.Abp.Metro.UI.TagHelpers.Core;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers.Pagination;

[HtmlTargetElement("metro-pagination", TagStructure = TagStructure.WithoutEndTag)]
public class MetroPaginationTagHelper : MetroTagHelper
{
    public MetroPaginationTagHelper(IStringLocalizer<AbpUiResource> l, HtmlEncoder htmlEncoder,
        IHtmlGenerator generator)
    {
        L = l;
        HtmlEncoder = htmlEncoder;
        Generator = generator;
    }

    protected const string PagerPreviousKey = "PagerPrevious";
    protected const string PageItemCls = "page-item";
    protected const string PagerNextKey = "PagerNext";

    protected IStringLocalizer<AbpUiResource> L { get; }
    protected HtmlEncoder HtmlEncoder { get; }
    protected IHtmlGenerator Generator { get; }
    public PagerModel Model { get; set; }
    public PaginationSize Size { get; set; } = PaginationSize.Default;
    public MetroAccentColor AccentColor { get; set; } = MetroAccentColor.Default;
    public bool Rounded { get; set; } = false;
    public bool Gap { get; set; } = true;
    public PaginationAlignment PaginationAlign { get; set; } = PaginationAlignment.Left;
    public bool ShowInfo { get; set; } = false;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (Model.ShownItemsCount <= 0)
        {
            output.SuppressOutput();
        }

        await ProcessMainTagAsync(context, output);
        await SetContentAsHtmlAsync(context, output);
    }

    protected virtual async Task SetContentAsHtmlAsync(TagHelperContext context, TagHelperOutput output)
    {
        var html = new StringBuilder("");

        if (PaginationAlign == PaginationAlignment.Right || PaginationAlign == PaginationAlignment.Center)
            html.AppendLine(await GetShowInfoAsync());

        html.AppendLine(await GetPreviousButtonAsync(context, output));
        html.AppendLine(await GetPagesAsync(context, output));
        html.AppendLine(await GetNextButton(context, output));

        if (PaginationAlign == PaginationAlignment.Left) html.AppendLine(await GetShowInfoAsync());

        output.Content.SetHtmlContent(html.ToString());
    }

    protected virtual Task ProcessMainTagAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.AddClass("pagination");
        switch (PaginationAlign)
        {
            case PaginationAlignment.Right:
                output.Attributes.AddClass("flex-justify-end");
                break;
            case PaginationAlignment.Center:
                output.Attributes.AddClass("flex-justify-center");
                break;
            case PaginationAlignment.Left:
            default:
                break;
        }

        if (Size != PaginationSize.Default)
        {
            output.Attributes.AddClass("size-" + Size.ToString().ToLowerInvariant());
        }

        if (AccentColor != MetroAccentColor.Default)
        {
            output.Attributes.AddClass(AccentColor.ToString().ToLowerInvariant());
        }

        if (Rounded)
        {
            output.Attributes.AddClass("rounded");
        }

        if (!Gap)
        {
            output.Attributes.AddClass("no-gap");
        }

        return Task.CompletedTask;
    }

    protected virtual Task<string> GetShowInfoAsync()
    {
        var builder = new TagBuilder("li");
        if (!ShowInfo) return Task.FromResult(builder.ToHtmlString());
        builder.AddCssClass("info-item");
        builder.AddCssClass("no-link");
        if (PaginationAlign == PaginationAlignment.Left) builder.AddCssClass("text-right");
        if (PaginationAlign == PaginationAlignment.Center)
        {
            builder.AddCssClass("text-center");
            builder.AddCssClass("w-100");
        }

        var info = HtmlEncoder.Encode(L["PagerInfo{0}{1}{2}", Model.ShowingFrom, Model.ShowingTo,
            Model.TotalItemsCount]);
        builder.InnerHtml.AppendHtml($"<a class=\"page-link\">{info}</a>");

        return Task.FromResult(builder.ToHtmlString());
    }

    protected virtual async Task<string> GetPagesAsync(TagHelperContext context, TagHelperOutput output)
    {
        var pagesHtml = new StringBuilder("");

        foreach (var page in Model.Pages)
        {
            pagesHtml.AppendLine(await GetPageAsync(context, output, page));
        }

        return pagesHtml.ToString();
    }

    protected virtual async Task<string> GetPageAsync(TagHelperContext context, TagHelperOutput output, PageItem page)
    {
        var builder = new TagBuilder("li");
        builder.AddCssClass(PageItemCls);
        if (Model.CurrentPage == page.Index) builder.AddCssClass("active");

        if (page.IsGap)
        {
            builder.AddCssClass("no-link");
            builder.InnerHtml.AppendHtml($"<a class=\"page-link\">...</a>");
        }
        else if (!page.IsGap && Model.CurrentPage == page.Index)
        {
            builder.InnerHtml.AppendHtml($"<a class=\"page-link\" href=\"#\">{page.Index}</a>");
        }
        else
        {
            builder.InnerHtml.AppendHtml(await RenderAnchorTagHelperLinkHtmlAsync(context, output,
                page.Index.ToString(), page.Index.ToString()));
        }

        return builder.ToHtmlString();
    }

    protected virtual async Task<string> GetPreviousButtonAsync(TagHelperContext context, TagHelperOutput output)
    {
        var currentPage = Model.CurrentPage == 1
            ? Model.CurrentPage.ToString()
            : (Model.CurrentPage - 1).ToString();
        var builder = new TagBuilder("li");
        builder.AddCssClass(PageItemCls);
        if (Model.CurrentPage == 1) builder.AddCssClass("disabled");
        builder.InnerHtml.AppendHtml(
            await RenderAnchorTagHelperLinkHtmlAsync(context, output, currentPage, PagerPreviousKey));
        return builder.ToHtmlString();
    }

    protected virtual async Task<string> GetNextButton(TagHelperContext context, TagHelperOutput output)
    {
        var builder = new TagBuilder("li");
        builder.AddCssClass(PageItemCls);
        var currentPage = (Model.CurrentPage + 1).ToString();
        if (Model.CurrentPage == Model.TotalPageCount) builder.AddCssClass("disabled");
        builder.InnerHtml.AppendHtml(
            await RenderAnchorTagHelperLinkHtmlAsync(context, output, currentPage, PagerNextKey));
        return builder.ToHtmlString();
    }

    protected virtual Task<string> RenderAnchorTagHelperLinkHtmlAsync(TagHelperContext context,
        TagHelperOutput output, string currentPage, string localizationKey)
    {
        var builder = new TagBuilder("a");
        builder.AddCssClass("page-link");
        var routeValue =
            $"pageIndex={currentPage}{(Model.Sort.IsNullOrWhiteSpace() ? "" : "&sort=" + Model.Sort)}";
        builder.Attributes.Add("href", $"{Model.PageUrl}?{routeValue}");
        builder.InnerHtml.AppendHtml(L[localizationKey]);
        return Task.FromResult(builder.ToHtmlString());
    }
}