using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.TagHelpers;


public abstract class MetroTagHelper : TagHelper, ITransientDependency
{
}

public abstract class MetroTagHelper<TTagHelper, TService> : MetroTagHelper
    where TTagHelper : MetroTagHelper<TTagHelper, TService>
    where TService : class, IMetroTagHelperService<TTagHelper>
{
    protected TService Service { get; }

    public override int Order => Service.Order;

    [HtmlAttributeNotBound] 
    [ViewContext] 
    public ViewContext ViewContext { get; set; } 

    protected MetroTagHelper(TService service)
    {
        Service = service;
        Service.As<MetroTagHelperService<TTagHelper>>().TagHelper = (TTagHelper)this;
    }

    public override void Init(TagHelperContext context)
    {
        Service.Init(context);
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        Service.Process(context, output);
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        return Service.ProcessAsync(context, output);
    }
}
