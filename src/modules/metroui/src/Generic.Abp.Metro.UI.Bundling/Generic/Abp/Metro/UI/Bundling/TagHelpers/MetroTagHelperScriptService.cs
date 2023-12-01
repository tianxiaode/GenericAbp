using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class MetroTagHelperScriptService : MetroTagHelperResourceService
{
    public MetroTagHelperScriptService(IBundleManager bundleManager, IOptions<AbpBundlingOptions> options,
        IWebHostEnvironment hostingEnvironment, ILogger<MetroTagHelperScriptService> logger) : base(bundleManager,
        options, hostingEnvironment, logger)
    {
    }

    protected override void CreateBundle(string bundleName, List<BundleTagHelperItem> bundleItems)
    {
        Options.ScriptBundles.TryAdd(
            bundleName,
            configuration => bundleItems.ForEach(bi => bi.AddToConfiguration(configuration)),
            HostingEnvironment.IsDevelopment() && bundleItems.Any()
        );
    }

    protected override async Task<IReadOnlyList<string>> GetBundleFilesAsync(string bundleName)
    {
        return await BundleManager.GetScriptBundleFilesAsync(bundleName ?? string.Empty);
    }

    protected override void AddHtmlTag(ViewContext viewContext, TagHelper tagHelper, TagHelperContext context,
        TagHelperOutput output, string file)
    {
        var defer = tagHelper switch
        {
            MetroScriptTagHelper scriptTagHelper => scriptTagHelper.Defer,
            MetroScriptBundleTagHelper scriptBundleTagHelper => scriptBundleTagHelper.Defer,
            _ => false
        };

        var deferText = (defer || Options.DeferScriptsByDefault ||
                         Options.DeferScripts.Any(x => file.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
            ? "defer"
            : string.Empty;
        output.Content.AppendHtml(
            $"<script {deferText} src=\"{viewContext.GetUrlHelper().Content(file.EnsureStartsWith('~'))}\"></script>{Environment.NewLine}");
    }
}