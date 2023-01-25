using Generic.Abp.Metro.UI.Settings;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Settings;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelperService : MetroTagHelperService<MetroFormContentTagHelper>
{
    public MetroFormContentTagHelperService(HtmlEncoder htmlEncoder, IHtmlGenerator htmlGenerator, IServiceProvider serviceProvider, IStringLocalizer<AbpUiResource> localizer, ISettingProvider settingProvider, ILogger<MetroFormContentTagHelperService> logger)
    {
        HtmlEncoder = htmlEncoder;
        HtmlGenerator = htmlGenerator;
        ServiceProvider = serviceProvider;
        Localizer = localizer;
        SettingProvider = settingProvider;
        Logger = logger;
    }

    protected  HtmlEncoder HtmlEncoder { get; }
    protected  IHtmlGenerator HtmlGenerator { get; }
    protected  IServiceProvider ServiceProvider { get; }
    protected  IStringLocalizer<AbpUiResource> Localizer { get; }
    protected ISettingProvider SettingProvider { get; }
    protected ILogger<MetroFormContentTagHelperService> Logger { get; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await CreateFormContentAsync(context);

        output.TagName = "div";
        output.Attributes.AddClass("row");

        var childContent = await output.GetChildContentAsync();

        output.Content.AppendHtml(childContent);
        await ProcessFieldsAsync(context, output);

        SetContent(context, output, childContent);
    }

    protected virtual void SetContent(TagHelperContext context, TagHelperOutput output,  TagHelperContent childContent)
    {
        var formItems = (List<FormItem>)context.Items[FormItems] ?? new List<FormItem>();
        var contentBuilder = new StringBuilder("");

        foreach (var item in formItems.OrderBy(o => o.Order))
        {
            contentBuilder.AppendLine(item.HtmlContent);
        }

        var content = childContent.GetContent();
        content = contentBuilder + content;

        output.Content.SetHtmlContent(content);
    }

    protected virtual async Task CreateFormContentAsync(TagHelperContext context)
    {
        var cols = await SettingProvider.GetAsync(MetroUiSettings.FormDefaultCols, 2);
        var horizontal = await SettingProvider.GetAsync(MetroUiSettings.FormHorizontal, true);
        var settingWidth = await SettingProvider.GetAsync(MetroUiSettings.FormDefaultLabelWidth, 100);
        var labelWidth = (int)LabelWidth.W150 == settingWidth ? LabelWidth.W150 :
            (int)LabelWidth.W120 == settingWidth ? LabelWidth.W120 :
            (int)LabelWidth.W130 == settingWidth ? LabelWidth.W130 :
            (int)LabelWidth.W140 == settingWidth ? LabelWidth.W140 : LabelWidth.W100;
        if (TagHelper.Cols != null) cols = TagHelper.Cols.Value;
        if(TagHelper.Horizontal != null) horizontal = TagHelper.Horizontal.Value;
        if(TagHelper.LabelWidth != null) labelWidth = TagHelper.LabelWidth.Value;
        context.Items["FormContent"] = new FormContent(cols, horizontal, labelWidth, TagHelper.RequiredSymbols ?? true);

    }

    protected virtual async Task ProcessFieldsAsync(TagHelperContext context, TagHelperOutput output)
    {
        if(TagHelper.Model?.ModelExplorer?.Properties == null) return;
        await CreateItemsAsync<FormItem>(context, FormItems);
        var models = await GetModelsAsync(context, output);
        var exits = (List<FormItem>) context.Items[FormItems];
        if (exits != null)
        {
            var names = exits.Select(x => x.Name).ToList();
            models = models.Where(m => !names.Contains(m.Name)).ToList();
        }

        foreach (var model in models)
        {
            if (IsCheckboxGroup(model.ModelExplorer))
            {
                await ProcessCheckboxGroupAsync(context, output, model);
            }
            else if(IsRadioGroup(model.ModelExplorer))
            {
                await ProcessRadioGroupAsync(context, output, model);
            }
            else if (IsSelectGroup(context, model))
            {
                await ProcessSelectGroupAsync(context, output, model);
            }
            else
            {
                await ProcessInputAsync(context, output, model);
            }
        }
    }

    protected virtual async Task ProcessCheckboxGroupAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model)
    {
        var checkboxGroupAttribute = model.ModelExplorer.GetAttribute<MetroCheckboxGroup>();
        var tagHelper = ServiceProvider.GetRequiredService<MetroCheckboxGroupTagHelper>();
        tagHelper.AspFor = model;
        tagHelper.AspItems = null;
        tagHelper.ViewContext = TagHelper.ViewContext;
        tagHelper.Disabled = checkboxGroupAttribute.Disabled;
        tagHelper.Cols = checkboxGroupAttribute.Cols;

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
        output.Content.AppendHtml(html);

    }

    protected virtual async Task ProcessRadioGroupAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model)
    {
        var radioGroupAttribute = model.ModelExplorer.GetAttribute<MetroRadioGroup>();
        var tagHelper = ServiceProvider.GetRequiredService<MetroRadioGroupTagHelper>();
        tagHelper.AspFor = model;
        tagHelper.AspItems = null;
        tagHelper.ViewContext = TagHelper.ViewContext;
        tagHelper.Disabled = radioGroupAttribute.Disabled;
        tagHelper.Cols = radioGroupAttribute.Cols;

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
        output.Content.AppendHtml(html);

    }


    protected virtual async Task ProcessSelectGroupAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model)
    {
        var tagHelper = ServiceProvider.GetRequiredService<MetroSelectTagHelper>();
        tagHelper.AspFor = model;
        tagHelper.AspItems = null;
        tagHelper.ViewContext = TagHelper.ViewContext;

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
        output.Content.AppendHtml(html);

    }

    protected virtual async Task ProcessInputAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    {
        var tagHelper = ServiceProvider.GetRequiredService<MetroInputTagHelper>();
        tagHelper.AspFor = model;
        tagHelper.ViewContext = TagHelper.ViewContext;

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
        output.Content.AppendHtml(html);
    }


    protected virtual Task<List<ModelExpression>> GetModelsAsync(TagHelperContext context, TagHelperOutput output)
    {
        return Task.FromResult(TagHelper.Model.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively));
    }

    protected virtual List<ModelExpression> ExploreModelsRecursively(List<ModelExpression> list, ModelExplorer model)
    {
        if (model.GetAttribute<DynamicFormIgnore>() != null)
        {
            return list;
        }
        if (!IsCsharpClassOrPrimitive(model.ModelType) && !model.Metadata.IsEnumerableType)
            return IsListOfSelectItem(model.ModelType)
                ? list
                : model.Properties.Aggregate(list, ExploreModelsRecursively);
        list.Add(ModelExplorerToModelExpressionConverter(model));

        return list;

    }

    protected virtual ModelExpression ModelExplorerToModelExpressionConverter(ModelExplorer explorer)
    {
        var temp = explorer;
        var propertyName = explorer.Metadata.PropertyName;

        while (temp?.Container?.Metadata?.PropertyName != null)
        {
            temp = temp.Container;
            propertyName = temp.Metadata.PropertyName + "." + propertyName;
        }

        return new ModelExpression(propertyName, explorer);
    }

    protected virtual bool IsCsharpClassOrPrimitive(Type type)
    {
        if (type == null)
        {
            return false;
        }

        return type.IsPrimitive ||
               type.IsValueType ||
               type == typeof(string) ||
               type == typeof(Guid) ||
               type == typeof(DateTime) ||
               type == typeof(ValueType) ||
               type == typeof(TimeSpan) ||
               type == typeof(DateTimeOffset) ||
               type.IsEnum;
    }

    protected virtual bool IsListOfSelectItem(Type type)
    {
        return type == typeof(List<SelectListItem>) || type == typeof(IEnumerable<SelectListItem>);
    }

    protected virtual bool IsSelectGroup(TagHelperContext context, ModelExpression model)
    {
        return IsEnum(model.ModelExplorer) || AreSelectItemsProvided(model.ModelExplorer);
    }

    protected virtual bool IsEnum(ModelExplorer explorer)
    {
        return explorer.Metadata.IsEnum;
    }

    protected virtual bool AreSelectItemsProvided(ModelExplorer explorer)
    {
        return explorer.GetAttribute<SelectItems>() != null;
    }

    protected virtual bool IsRadioGroup(ModelExplorer explorer)
    {
        return explorer.GetAttribute<MetroRadioGroup>() != null;
    }

    protected virtual bool IsCheckboxGroup(ModelExplorer explorer)
    {
        return explorer.GetAttribute<MetroCheckboxGroup>() != null;
    }

}
