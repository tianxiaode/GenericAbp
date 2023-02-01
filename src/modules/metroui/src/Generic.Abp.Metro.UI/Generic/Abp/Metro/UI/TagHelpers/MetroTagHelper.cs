using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers;

public abstract class MetroTagHelper : TagHelper
{
    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }
    protected int OrderIncrement { get; set; } = 0;

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

    protected virtual async Task AddGroupItemAsync(TagHelperContext context, string name, int displayOrder,
        string htmlContent)
    {
        var items = await GetGroupItems(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        await AddGroupItemAsync(items, name, displayOrder, htmlContent);
    }

    protected virtual Task AddGroupItemAsync(List<T> items, string name, int displayOrder,
        string htmlContent)
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