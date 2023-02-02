using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Generic.Abp.Metro.UI.TagHelpers;

public abstract class MetroTagHelper : TagHelper
{
    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }
    protected int OrderIncrement { get; set; } = 0;


    protected virtual Task AppendHtmlAsync<T>(T builder, string html) where T : class
    {
        if (string.IsNullOrWhiteSpace(html)) return Task.CompletedTask;
        switch (builder)
        {
            case TagBuilder tagBuilder:
                tagBuilder.InnerHtml.AppendHtml(html);
                break;
            case TagHelperOutput output:
                output.Content.AppendHtml(html);
                break;
        }

        return Task.CompletedTask;
    }

    protected virtual Task AddClassAsync<T>(T builder, string cls) where T : class
    {
        if (string.IsNullOrWhiteSpace(cls)) return Task.CompletedTask;
        switch (builder)
        {
            case TagBuilder tagBuilder:
                tagBuilder.AddCssClass(cls);
                break;
            case TagHelperOutput output:
                output.Attributes.AddClass(cls);
                break;
        }

        return Task.CompletedTask;
    }

    protected virtual Task AddAttributeAsync<T>(T builder, string name, string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return Task.CompletedTask;
        switch (builder)
        {
            case TagBuilder tagBuilder:
                tagBuilder.Attributes.Add(name, value);
                break;
            case TagHelperOutput output:
                output.Attributes.Add(name, value);
                break;
        }

        return Task.CompletedTask;
    }

    protected virtual Task<int> GetDisplayOrderAsync(int order)
    {
        if (order != 0) return Task.FromResult(order);
        order = TagHelperConsts.DisplayOrder + OrderIncrement;
        OrderIncrement += 100;

        return Task.FromResult(order);
    }

    protected virtual Task SetDisplayOrderAsync(TagBuilder tagBuilder, int order)
    {
        if (order != 0) tagBuilder.Attributes.Add("style", $"order:{order}");
        return Task.CompletedTask;
    }

    protected virtual Task AddStyleAsync(TagHelperOutput output, string value)
    {
        var attributes = output.Attributes;
        if (attributes.TryGetAttribute("style", out var styleAttribute))
        {
            var currentValue = styleAttribute.Value?.ToString();
            if (!string.IsNullOrEmpty(currentValue))
            {
                attributes.Add("style", currentValue.EnsureEndsWith(';') + value.EnsureEndsWith(';'));
            }
        }
        else
        {
            attributes.Add("style", value);
        }

        return Task.CompletedTask;
    }
}

public abstract class MetroTagHelper<T> : MetroTagHelper where T : IGroupItem, new()
{
    protected string GroupItemsName { get; set; }

    protected virtual void InitGroupItems(TagHelperContext context)
    {
        context.Items.Add(GroupItemsName, new List<T>());
    }

    protected virtual Task<List<T>> GetGroupItems(TagHelperContext context)
    {
        return Task.FromResult((List<T>)context.Items[GroupItemsName]);
    }

    protected virtual Task<bool> HasGroupItemsAsync(TagHelperContext context)
    {
        return Task.FromResult(context.Items.Any(m => m.Key.ToString() == GroupItemsName));
    }

    protected virtual async Task<bool> GroupItemsAnyAsync(TagHelperContext context, string name)
    {
        var items = await GetGroupItems(context);
        return items != null && items.Any(m => m.Name == name);
    }

    protected virtual async Task AddGroupItemAsync(TagHelperContext context, string name, string htmlContent,
        int displayOrder = 0)
    {
        var items = await GetGroupItems(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        await AddGroupItemAsync(items, name, htmlContent, displayOrder);
    }

    protected virtual Task AddGroupItemAsync(List<T> items, string name, string htmlContent, int displayOrder = 0)
    {
        var item = new T
        {
            HtmlContent = htmlContent,
            DisplayOrder = displayOrder,
            Name = name,
        };
        items.Add(item);
        return Task.CompletedTask;
    }

    protected virtual async Task AddGroupItemAsync(TagHelperContext context, T item)
    {
        var items = await GetGroupItems(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        items.Add(item);
    }
}