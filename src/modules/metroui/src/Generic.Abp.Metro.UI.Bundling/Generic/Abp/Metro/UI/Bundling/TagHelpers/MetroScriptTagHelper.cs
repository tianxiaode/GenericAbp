using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

[HtmlTargetElement("metro-script", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroScriptTagHelper : MetroBundleItemTagHelper, IBundleItemTagHelper
{
    public MetroScriptTagHelper(MetroTagHelperScriptService resourceService) : base(
        resourceService)
    {
    }

    [HtmlAttributeName("defer")] public bool Defer { get; set; }

    protected override string GetFileExtension()
    {
        return "js";
    }
}