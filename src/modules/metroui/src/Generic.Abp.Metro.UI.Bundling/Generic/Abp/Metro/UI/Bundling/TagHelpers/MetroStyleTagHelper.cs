using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-style", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroStyleTagHelper : MetroBundleItemTagHelper, IBundleItemTagHelper
{
    public MetroStyleTagHelper(MetroTagHelperStyleService resourceService) : base(
        resourceService)
    {
    }

    [HtmlAttributeName("preload")] public bool Preload { get; set; }


    protected override string GetFileExtension()
    {
        return "css";
    }
}