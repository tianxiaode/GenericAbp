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

[HtmlTargetElement("metro-script-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
public class MetroScriptBundleTagHelper : MetroBundleTagHelper, IBundleTagHelper
{
    [HtmlAttributeName("defer")] public bool Defer { get; set; }


    public MetroScriptBundleTagHelper(MetroTagHelperScriptService resourceService) : base(
        resourceService)
    {
    }
}