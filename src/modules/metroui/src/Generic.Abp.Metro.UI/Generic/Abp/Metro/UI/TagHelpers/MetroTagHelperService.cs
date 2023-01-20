using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace Generic.Abp.Metro.UI.TagHelpers;

//TODO: Refactor this class, extract bootstrap functionality!
public abstract class MetroTagHelperService<TTagHelper> : IMetroTagHelperService<TTagHelper>
    where TTagHelper : TagHelper
{
    protected const string FormGroupContents = "FormGroupContents";
    protected const string TabItems = "TabItems";
    protected const string AccordionItems = "AccordionItems";
    protected const string BreadcrumbItemsContent = "BreadcrumbItemsContent";
    protected const string CarouselItemsContent = "CarouselItemsContent";
    protected const string TabItemsDataTogglePlaceHolder = "{_data_toggle_Placeholder_}";
    protected const string TabItemNamePlaceHolder = "{_Tab_Tag_Name_Placeholder_}";
    //protected const string MetroFormContentPlaceHolder = "{_MetroFormContentPlaceHolder_}";
    protected const string MetroTabItemActivePlaceholder = "{_Tab_Active_Placeholder_}";
    protected const string MetroTabDropdownItemsActivePlaceholder = "{_Tab_DropDown_Items_Placeholder_}";
    protected const string MetroTabItemShowActivePlaceholder = "{_Tab_Show_Active_Placeholder_}";
    protected const string MetroBreadcrumbItemActivePlaceholder = "{_Breadcrumb_Active_Placeholder_}";
    protected const string MetroCarouselItemActivePlaceholder = "{_CarouselItem_Active_Placeholder_}";
    protected const string MetroTabItemSelectedPlaceholder = "{_Tab_Selected_Placeholder_}";
    protected const string MetroAccordionParentIdPlaceholder = "{_Parent_Accordion_Id_}";

    public TTagHelper TagHelper { get; internal set; }

    public virtual int Order { get; }

    public virtual void Init(TagHelperContext context)
    {

    }

    public virtual void Process(TagHelperContext context, TagHelperOutput output)
    {

    }

    public virtual Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        Process(context, output);
        return Task.CompletedTask;
    }


}
