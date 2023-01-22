using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System;
using Volo.Abp.DependencyInjection;
using Generic.Abp.Metro.UI.TagHelpers.Form;

namespace Generic.Abp.Metro.UI.TagHelpers.Dialog;

public abstract class MetroDialogTagHelperBase : FormTagHelper, ITransientDependency
{
    protected MetroDialogTagHelperBase(IHtmlGenerator generator) : base(generator)
    {
    }
}

public abstract class MetroDialogTagHelperBase<TTagHelper, TService> : MetroDialogTagHelperBase
    where TTagHelper : MetroDialogTagHelperBase<TTagHelper, TService>
    where TService : class, IMetroTagHelperService<TTagHelper>
{
    protected MetroDialogTagHelperBase(IHtmlGenerator generator, TService service) : base(generator)
    {
        Service = service;
        Service.As<MetroTagHelperService<TTagHelper>>().TagHelper = (TTagHelper)this;
    }

    protected TService Service { get; }

    public override int Order => Service.Order;


    public override void Init(TagHelperContext context)
    {
        Service.Init(context);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
        Service.Process(context, output);
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await base.ProcessAsync(context, output);
        await Service.ProcessAsync(context, output);
    }

}

public class MetroDialogTagHelper : MetroDialogTagHelperBase<MetroDialogTagHelper, MetroDialogTagHelperService>
{
    public MetroDialogButtons? Buttons { get; set; }
    public ButtonsAlign ButtonAlignment { get; set; } = ButtonsAlign.End;

    public MetroDialogTagHelper(IHtmlGenerator generator, MetroDialogTagHelperService service) : base(generator, service)
    {
    }
}
