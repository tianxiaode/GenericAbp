using Generic.Abp.Metro.UI.Settings;
using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Generic.Abp.Metro.UI.TagHelpers.Form.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelper : MetroTagHelper<FormGroupItem>
{
    public MetroFormContentTagHelper(HtmlEncoder htmlEncoder,
        IHtmlGenerator generator,
        IMetroTagHelperLocalizerService localizerService,
        ISettingProvider settingProvider,
        ILogger<MetroFormContentTagHelper> logger,
        SelectItemsService selectItemsService) : base(htmlEncoder)
    {
        Generator = generator;
        Localizer = localizerService;
        SettingProvider = settingProvider;
        Logger = logger;
        SelectItemsService = selectItemsService;

        GroupItemsName = TagHelperConsts.FormGroupItems;
    }

    protected IHtmlGenerator Generator { get; }
    protected IMetroTagHelperLocalizerService Localizer { get; }
    protected SelectItemsService SelectItemsService { get; }
    protected ISettingProvider SettingProvider { get; }
    protected ILogger<MetroFormContentTagHelper> Logger { get; }

    [HtmlAttributeName("metro-model")] public ModelExpression Model { get; set; }
    public int? Cols { get; set; }
    public bool? Horizontal { get; set; }
    public int LabelWidth { get; set; }
    public bool? RequiredSymbols { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await CreateFormContentAsync(context);

        output.TagName = "div";
        output.Attributes.AddClass("row");

        if (Model == null)
        {
            return;
        }

        var list = await InitGroupItemsAsync(context);
        var childContent = await output.GetChildContentAsync();

        await ProcessFieldsAsync(context, output, list);

        await SetContentAsync(context, output, childContent, list);
    }

    protected virtual Task SetContentAsync(TagHelperContext context, TagHelperOutput output,
        TagHelperContent childContent, List<FormGroupItem> list)
    {
        var contentBuilder = new StringBuilder("");

        foreach (var item in list.OrderBy(o => o.DisplayOrder))
        {
            contentBuilder.AppendLine(item.HtmlContent);
        }

        contentBuilder.AppendLine(childContent.GetContent());

        output.Content.SetHtmlContent(contentBuilder.ToString());
        return Task.CompletedTask;
    }

    protected virtual async Task CreateFormContentAsync(TagHelperContext context)
    {
        var cols = await SettingProvider.GetAsync(MetroUiSettings.FormDefaultCols, 2);
        var horizontal = await SettingProvider.GetAsync(MetroUiSettings.FormHorizontal, true);

        if (Cols != null) cols = Cols.Value;
        if (Horizontal != null) horizontal = Horizontal.Value;
        context.Items["FormContent"] = new FormContent(cols, horizontal, RequiredSymbols ?? true, LabelWidth);
    }

    protected virtual async Task ProcessFieldsAsync(TagHelperContext context, TagHelperOutput output,
        List<FormGroupItem> list)
    {
        if (Model.ModelExplorer?.Properties == null) return;
        var models = await GetModelsAsync(context, output);
        var exits = await GetGroupItemsAsync(context);
        if (exits != null)
        {
            var names = exits.Select(x => x.Name).ToList();
            models = models.Where(m => !names.Contains(m.Name)).ToList();
        }

        foreach (var model in models)
        {
            if (IsCheckboxGroup(model.ModelExplorer))
            {
                await ProcessCheckboxGroupAsync(context, output, model, list);
            }
            else if (IsRadioGroup(model.ModelExplorer))
            {
                await ProcessRadioGroupAsync(context, output, model, list);
            }
            else if (IsSelectGroup(context, model))
            {
                await ProcessSelectGroupAsync(context, output, model, list);
            }
            else
            {
                await ProcessInputAsync(context, output, model, list);
            }
        }
    }

    protected virtual async Task ProcessCheckboxGroupAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model, List<FormGroupItem> list)
    {
        var checkboxGroupAttribute = model.ModelExplorer.GetAttribute<CheckboxGroup>();
        var tagHelper = new MetroCheckboxGroupTagHelper(HtmlEncoder, Generator, Localizer, SelectItemsService)
        {
            AspFor = model,
            AspItems = null,
            ViewContext = ViewContext,
            IsDisabled = checkboxGroupAttribute.Disabled,
            Cols = checkboxGroupAttribute.Cols
        };

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div",
            TagMode.StartTagAndEndTag);
        await AddGroupItemAsync(list, model.Name, html, await GetDisplayOrderAsync());
    }

    protected virtual async Task ProcessRadioGroupAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model, List<FormGroupItem> list)
    {
        var radioGroupAttribute = model.ModelExplorer.GetAttribute<RadioGroup>();
        var tagHelper = new MetroCheckboxGroupTagHelper(HtmlEncoder, Generator, Localizer, SelectItemsService)
        {
            AspFor = model,
            AspItems = null,
            ViewContext = ViewContext,
            IsDisabled = radioGroupAttribute.Disabled,
            Cols = radioGroupAttribute.Cols
        };

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div",
            TagMode.StartTagAndEndTag);
        await AddGroupItemAsync(list, model.Name, html, await GetDisplayOrderAsync());
    }


    protected virtual async Task ProcessSelectGroupAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model, List<FormGroupItem> list)
    {
        var tagHelper = new MetroSelectTagHelper(HtmlEncoder, Generator, Localizer, SelectItemsService)
        {
            AspFor = model,
            AspItems = null,
            ViewContext = ViewContext
        };

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div",
            TagMode.StartTagAndEndTag);
        await AddGroupItemAsync(list, model.Name, html, await GetDisplayOrderAsync());
    }

    protected virtual async Task ProcessInputAsync(TagHelperContext context, TagHelperOutput output,
        ModelExpression model, List<FormGroupItem> list)
    {
        var tagHelper = new MetroInputTagHelper(HtmlEncoder, Localizer, Generator)
        {
            AspFor = model,
            ViewContext = ViewContext
        };

        var html = await tagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div",
            TagMode.StartTagAndEndTag);
        await AddGroupItemAsync(list, model.Name, html, await GetDisplayOrderAsync());
    }


    protected virtual Task<List<ModelExpression>> GetModelsAsync(TagHelperContext context, TagHelperOutput output)
    {
        return Task.FromResult(
            Model.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively));
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
        return explorer.GetAttribute<RadioGroup>() != null;
    }

    protected virtual bool IsCheckboxGroup(ModelExplorer explorer)
    {
        return explorer.GetAttribute<CheckboxGroup>() != null;
    }
}