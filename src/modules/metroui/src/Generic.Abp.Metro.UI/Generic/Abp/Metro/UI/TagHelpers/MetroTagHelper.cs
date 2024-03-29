using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.TagHelpers;

public abstract class MetroTagHelper : TagHelper
{
    protected const string DataAttributePrefix = "data-";
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

    protected virtual async Task AddDataAttributeAsync<T>(T builder, string name, object value, bool isLower = true,
        bool isToKebabCase = false)
    {
        if (string.IsNullOrWhiteSpace(name) || value == null) return;
        var strValue = await ValueToStringAsync(value);
        if (string.IsNullOrWhiteSpace(strValue)) return;
        name = $"{DataAttributePrefix}{name.ToKebabCase()}";
        if (isToKebabCase) strValue = strValue.ToKebabCase();
        if (isLower) strValue = strValue.ToLowerInvariant();
        await AddAttributeAsync(builder, name, strValue);
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

    protected virtual Task<int> GetDisplayOrderAsync(int order = 0)
    {
        if (order is not (0 or TagHelperConsts.DisplayOrder)) return Task.FromResult(order);
        order = TagHelperConsts.DisplayOrder + OrderIncrement;
        OrderIncrement += 100;

        return Task.FromResult(order);
    }

    protected virtual async Task SetDisplayOrderAsync(TagBuilder tagBuilder, int order)
    {
        if (order == 0) return;
        await AddStyleAsync(tagBuilder, $"order:{order};");
    }

    protected virtual async Task AddStyleAsync<TBuilder>(TBuilder builder, string value)
    {
        switch (builder)
        {
            case TagHelperOutput output:
                await AddStyleAsync(output, value);
                break;
            case TagBuilder tagBuilder:
                await AddStyleAsync(tagBuilder, value);
                break;
        }
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
            else
            {
                attributes.Add("style", value);
            }
        }
        else
        {
            attributes.Add("style", value);
        }

        return Task.CompletedTask;
    }

    protected virtual Task AddStyleAsync(TagBuilder builder, string value)
    {
        var attributes = builder.Attributes;
        if (attributes.TryGetValue("style", out var currentValue))
        {
            if (!string.IsNullOrEmpty(currentValue))
            {
                attributes.Add("style", currentValue.EnsureEndsWith(';') + value.EnsureEndsWith(';'));
            }
            else
            {
                attributes.Add("style", value);
            }
        }
        else
        {
            attributes.Add("style", value);
        }

        return Task.CompletedTask;
    }

    protected virtual Task<string> ValueToStringAsync(object value)
    {
        var type = value.GetType();
        var str = type.IsEnum ? Enum.GetName(type, value) : value.ToString();
        return Task.FromResult(str);
    }

    #region depth item

    protected virtual async Task<List<string>> AddFlagListAsync(TagHelperContext context, string itemName)
    {
        var name = await GetItemKeyNameAsync(itemName, isDepth: true);
        if (!context.Items.ContainsKey(name))
        {
            context.Items[name] = 0;
        }

        var depth = (int)context.Items[name] + 1;
        context.Items[context.UniqueId] = depth;
        context.Items[name] = depth;
        var flagName = await GetItemKeyNameAsync(itemName, isFlag: true);
        var list = new List<string>();
        context.Items[flagName] = list;
        return list;
    }

    protected virtual async Task AddFlagValueAsync(TagHelperContext context, string itemName, string value)
    {
        var name = await GetItemKeyNameAsync(itemName, isFlag: true);
        if (!context.Items.ContainsKey(name)) return;
        var depthName = await GetItemKeyNameAsync(itemName, isDepth: true);
        if (context.Items.TryGetValue(depthName, out var depth))
        {
            var list = context.Items[name] as List<string>;
            list?.Add($"{value}_{depth}");
        }
    }


    protected virtual Task<string> GetItemKeyNameAsync(string itemName, bool isDepth = false, bool isGroup = false,
        bool isFlag = true)
    {
        var result = itemName + "_";
        if (isDepth) result += "depth";
        if (isGroup) result += "group";
        if (isFlag) result += "flag";
        return Task.FromResult(result);
    }

    #endregion
}

public abstract class MetroTagHelper<T> : MetroTagHelper where T : IGroupItem, new()
{
    protected MetroTagHelper(HtmlEncoder htmlEncoder)
    {
        HtmlEncoder = htmlEncoder;
    }

    protected HtmlEncoder HtmlEncoder { get; }
    protected string GroupItemsName { get; set; }

    protected virtual Task<List<T>> InitGroupItemsAsync(TagHelperContext context)
    {
        var list = new List<T>();
        if (context.Items.ContainsKey(GroupItemsName))
        {
            list = context.Items[GroupItemsName] as List<T>;
        }
        else
        {
            context.Items.Add(GroupItemsName, list);
        }

        return Task.FromResult(list);
    }

    protected virtual async Task<int> GetItemDepthAsync(TagHelperContext context)
    {
        var name = await GetDepthNameAsync();
        if (context.Items.ContainsKey(name)) return (int)context.Items[name];
        return 0;
    }

    protected virtual Task<string> GetDepthNameAsync()
    {
        return Task.FromResult($"{GroupItemsName}_depth");
    }

    protected virtual Task<List<T>> GetGroupItemsAsync(TagHelperContext context)
    {
        return Task.FromResult((List<T>)context.Items[GroupItemsName]);
    }

    [ItemCanBeNull]
    protected virtual async Task<T> GetGroupItemAsync(TagHelperContext context, string name)
    {
        var items = await GetGroupItemsAsync(context);
        return items == null ? default! : items.FirstOrDefault(m => m.Name == name);
    }

    protected virtual Task<bool> HasGroupItemsAsync(TagHelperContext context)
    {
        return Task.FromResult(context.Items.ContainsKey(GroupItemsName));
    }

    protected virtual async Task<bool> GroupItemsAnyAsync(TagHelperContext context, string name)
    {
        var items = await GetGroupItemsAsync(context);
        return items != null && items.Any(m => m.Name == name);
    }

    protected virtual async Task AddGroupItemAsync(TagHelperContext context, string name, string htmlContent,
        int displayOrder = 0)
    {
        var items = await GetGroupItemsAsync(context);
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
        var items = await GetGroupItemsAsync(context);
        if (items == null) throw new ArgumentNullException(nameof(items));
        items.Add(item);
    }

    protected virtual async Task<string> GetBuilderAsHtmlAsync<TBuilder>(TBuilder builder)
    {
        var html = builder switch
        {
            TagBuilder tagBuilder => tagBuilder.ToHtmlString(),
            TagHelperOutput output => await output.RenderAsync(HtmlEncoder),
            _ => ""
        };
        return html;
    }
}