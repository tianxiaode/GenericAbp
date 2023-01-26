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
    protected MetroTagHelper(IMetroTagHelperLocalizer tagHelperLocalizer)
    {
        TagHelperLocalizer = tagHelperLocalizer;
    }

    protected IMetroTagHelperLocalizer TagHelperLocalizer { get; }


    [HtmlAttributeNotBound] [ViewContext] public ViewContext ViewContext { get; set; }
}

public abstract class MetroTagHelper<T> : MetroTagHelper where T : IGroupItem, new()
{
    protected MetroTagHelper(IMetroTagHelperLocalizer tagHelperLocalizer) : base(tagHelperLocalizer)
    {
    }

    public string GroupItemsName { get; set; }

    public virtual Task InitGroupItemsAsync(TagHelperContext context)
    {
        context.Items.Add(GroupItemsName, new List<T>());
        return Task.CompletedTask;
    }

    public virtual Task<List<T>> GetGroupItems(TagHelperContext context)
    {
        return Task.FromResult((List<T>)context.Items[GroupItemsName]);
    }

    public virtual Task<bool> HasGroupItemsAsync(TagHelperContext context)
    {
        return Task.FromResult(context.Items.Any(m => m.Key.ToString() == GroupItemsName));
    }

    public virtual async Task<bool> GroupItemsAnyAsync(TagHelperContext context, string name)
    {
        var items = await GetGroupItems(context);
        return items != null && items.Any(m => m.Name == name);
    }

    public virtual async Task AddGroupItemAsync(TagHelperContext context, string name, int order,
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

    public virtual async Task AddGroupItemAsync(TagHelperContext context, T item)
    {
        var items = await GetGroupItems(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        items.Add(item);
    }
}