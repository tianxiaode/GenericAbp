﻿using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Widgets;

[Dependency(ReplaceServices = true)]
public class MetroViewComponentHelper : IViewComponentHelper, IViewContextAware, ITransientDependency
{
    protected MetroWidgetOptions Options { get; }
    protected IPageWidgetManager PageWidgetManager { get; }
    protected DefaultViewComponentHelper DefaultViewComponentHelper { get; }

    public MetroViewComponentHelper(
        DefaultViewComponentHelper defaultViewComponentHelper,
        IOptions<MetroWidgetOptions> widgetOptions,
        IPageWidgetManager pageWidgetManager)
    {
        DefaultViewComponentHelper = defaultViewComponentHelper;
        PageWidgetManager = pageWidgetManager;
        Options = widgetOptions.Value;
    }

    public virtual async Task<IHtmlContent> InvokeAsync(string name, object arguments)
    {
        var widget = Options.Widgets.Find(name);
        if (widget == null)
        {
            return await DefaultViewComponentHelper.InvokeAsync(name, arguments);
        }
        return await InvokeWidgetAsync(arguments, widget);
    }

    public virtual async Task<IHtmlContent> InvokeAsync(Type componentType, object arguments)
    {
        var widget = Options.Widgets.Find(componentType);
        if (widget == null)
        {
            return await DefaultViewComponentHelper.InvokeAsync(componentType, arguments);
        }

        return await InvokeWidgetAsync(arguments, widget);
    }

    public virtual void Contextualize(ViewContext viewContext)
    {
        DefaultViewComponentHelper.Contextualize(viewContext);
    }

    protected virtual async Task<IHtmlContent> InvokeWidgetAsync(object arguments, WidgetDefinition widget)
    {
        PageWidgetManager.TryAdd(widget);

        var wrapperAttributesBuilder = new StringBuilder($"class=\"abp-widget-wrapper\" data-widget-Name=\"{widget.Name}\"");

        if (widget?.RefreshUrl != null)
        {
            wrapperAttributesBuilder.Append($" data-refresh-url=\"{widget.RefreshUrl}\"");
        }

        if (widget is { AutoInitialize: true })
        {
            wrapperAttributesBuilder.Append(" data-widget-auto-init=\"true\"");
        }

        return new HtmlContentBuilder()
            .AppendHtml($"<div {wrapperAttributesBuilder}>")
            .AppendHtml(await DefaultViewComponentHelper.InvokeAsync(widget?.ViewComponentType ?? default!   , arguments))
            .AppendHtml("</div>");
    }
}
