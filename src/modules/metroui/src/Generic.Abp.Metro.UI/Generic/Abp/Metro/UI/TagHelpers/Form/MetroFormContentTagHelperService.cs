using Generic.Abp.Metro.UI.TagHelpers.Extensions;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Generic.Abp.Metro.UI.Settings;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Settings;

namespace Generic.Abp.Metro.UI.TagHelpers.Form;

public class MetroFormContentTagHelperService : MetroTagHelperService<MetroFormContentTagHelper>
{
    public MetroFormContentTagHelperService(HtmlEncoder htmlEncoder, IHtmlGenerator htmlGenerator, IServiceProvider serviceProvider, IStringLocalizer<AbpUiResource> localizer, ISettingProvider settingProvider)
    {
        HtmlEncoder = htmlEncoder;
        HtmlGenerator = htmlGenerator;
        ServiceProvider = serviceProvider;
        Localizer = localizer;
        SettingProvider = settingProvider;
    }

    protected  HtmlEncoder HtmlEncoder { get; }
    protected  IHtmlGenerator HtmlGenerator { get; }
    protected  IServiceProvider ServiceProvider { get; }
    protected  IStringLocalizer<AbpUiResource> Localizer { get; }
    protected ISettingProvider SettingProvider { get; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await CreateItemsAsync<FormItem>(context, FormItems);
        await CreateFormContentAsync(context);

        output.TagName = "div";
        output.Attributes.AddClass("row");

        var childContent = await output.GetChildContentAsync();
        //await ProcessFieldsAsync(context, output);

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
        context.Items["FormContent"] = new FormContent(cols, horizontal, labelWidth);

    }

    //protected virtual async Task ProcessFieldsAsync(TagHelperContext context, TagHelperOutput output)
    //{
    //    var models = await GetModelsAsync(context, output);

    //    foreach (var model in models)
    //    {
    //        if (IsSelectGroup(context, model))
    //        {
    //            await ProcessSelectGroupAsync(context, output, model);
    //        }
    //        else
    //        {
    //            await ProcessInputGroupAsync(context, output, model);
    //        }
    //    }
    //}

    //protected virtual async Task ProcessSelectGroupAsync(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    //{
    //    var abpSelectTagHelper = GetSelectGroupTagHelper(context, output, model);

    //    await abpSelectTagHelper.RenderAsync(new TagHelperAttributeList(), context, HtmlEncoder, "div", TagMode.StartTagAndEndTag);
    //}

    //protected virtual MetroTagHelper GetSelectGroupTagHelper(TagHelperContext context, TagHelperOutput output, ModelExpression model)
    //{
    //    return IsRadioGroup(model.ModelExplorer) ?
    //        GetAbpRadioInputTagHelper(model) :
    //        GetSelectTagHelper(model);
    //}

    //protected virtual MetroTagHelper GetAbpRadioInputTagHelper(ModelExpression model)
    //{
    //    var radioButtonAttribute = model.ModelExplorer.GetAttribute<MetroRadioButton>();
    //    var abpRadioInputTagHelper = ServiceProvider.GetRequiredService<MetroRadioInputTagHelper>();
    //    abpRadioInputTagHelper.AspFor = model;
    //    abpRadioInputTagHelper.AspItems = null;
    //    abpRadioInputTagHelper.Inline = radioButtonAttribute.Inline;
    //    abpRadioInputTagHelper.Disabled = radioButtonAttribute.Disabled;
    //    abpRadioInputTagHelper.ViewContext = TagHelper.ViewContext;
    //    return abpRadioInputTagHelper;
    //}

    //protected virtual MetroTagHelper GetSelectTagHelper(ModelExpression model)
    //{
    //    var abpSelectTagHelper = ServiceProvider.GetRequiredService<AbpSelectTagHelper>();
    //    abpSelectTagHelper.AspFor = model;
    //    abpSelectTagHelper.AspItems = null;
    //    abpSelectTagHelper.ViewContext = TagHelper.ViewContext;
    //    return abpSelectTagHelper;
    //}

    //protected virtual Task<List<ModelExpression>> GetModelsAsync(TagHelperContext context, TagHelperOutput output)
    //{
    //    return Task.FromResult(TagHelper.Model.ModelExplorer.Properties.Aggregate(new List<ModelExpression>(), ExploreModelsRecursively));
    //}

    //protected virtual  List<ModelExpression> ExploreModelsRecursively(List<ModelExpression> list, ModelExplorer model)
    //{
    //    if (model.GetAttribute<DynamicFormIgnore>() != null)
    //    {
    //        return list;
    //    }

    //    if (!IsCsharpClassOrPrimitive(model.ModelType) && !IsListOfCsharpClassOrPrimitive(model.ModelType))
    //        return IsListOfSelectItem(model.ModelType)
    //            ? list
    //            : model.Properties.Aggregate(list, ExploreModelsRecursively);
    //    list.Add(ModelExplorerToModelExpressionConverter(model));

    //    return list;

    //}

    //protected virtual ModelExpression ModelExplorerToModelExpressionConverter(ModelExplorer explorer)
    //{
    //    var temp = explorer;
    //    var propertyName = explorer.Metadata.PropertyName;

    //    while (temp?.Container?.Metadata?.PropertyName != null)
    //    {
    //        temp = temp.Container;
    //        propertyName = temp.Metadata.PropertyName + "." + propertyName;
    //    }

    //    return new ModelExpression(propertyName, explorer);
    //}

    //protected virtual bool IsListOfCsharpClassOrPrimitive(Type type)
    //{
    //    var genericType = type.GenericTypeArguments.FirstOrDefault();

    //    if (genericType == null || !IsCsharpClassOrPrimitive(genericType))
    //    {
    //        return false;
    //    }

    //    return type.ToString().StartsWith("System.Collections.Generic.IEnumerable`") || type.ToString().StartsWith("System.Collections.Generic.List`");
    //}

    //protected virtual bool IsCsharpClassOrPrimitive(Type type)
    //{
    //    if (type == null)
    //    {
    //        return false;
    //    }

    //    return type.IsPrimitive ||
    //           type.IsValueType ||
    //           type == typeof(string) ||
    //           type == typeof(Guid) ||
    //           type == typeof(DateTime) ||
    //           type == typeof(ValueType) ||
    //           type == typeof(TimeSpan) ||
    //           type == typeof(DateTimeOffset) ||
    //           type.IsEnum;
    //}

    //protected virtual bool IsListOfSelectItem(Type type)
    //{
    //    return type == typeof(List<SelectListItem>) || type == typeof(IEnumerable<SelectListItem>);
    //}

    //protected virtual bool IsSelectGroup(TagHelperContext context, ModelExpression model)
    //{
    //    return IsEnum(model.ModelExplorer) || AreSelectItemsProvided(model.ModelExplorer);
    //}

    //protected virtual bool IsEnum(ModelExplorer explorer)
    //{
    //    return explorer.Metadata.IsEnum;
    //}

    //protected virtual bool AreSelectItemsProvided(ModelExplorer explorer)
    //{
    //    return explorer.GetAttribute<SelectItems>() != null;
    //}

    //protected virtual bool IsRadioGroup(ModelExplorer explorer)
    //{
    //    return explorer.GetAttribute<MetroRadioButton>() != null;
    //}

}
