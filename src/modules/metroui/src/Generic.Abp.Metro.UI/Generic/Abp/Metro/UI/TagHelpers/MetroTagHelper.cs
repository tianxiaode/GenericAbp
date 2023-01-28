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
}

public abstract class MetroTagHelper<T> : MetroTagHelper where T : IGroupItem, new()
{
    protected string GroupItemsName { get; set; }

    protected virtual Task InitGroupItemsAsync(TagHelperContext context)
    {
        context.Items.Add(GroupItemsName, new List<T>());
        return Task.CompletedTask;
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

    protected virtual async Task AddGroupItemAsync(TagHelperContext context, string name, int order,
        string htmlContent)
    {
        var items = await GetGroupItems(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        var item = new T
        {
            HtmlContent = htmlContent,
            Order = order,
            Name = name,
        };
        items.Add(item);
    }

    protected virtual async Task AddGroupItemAsync(TagHelperContext context, T item)
    {
        var items = await GetGroupItems(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        items.Add(item);
    }
}