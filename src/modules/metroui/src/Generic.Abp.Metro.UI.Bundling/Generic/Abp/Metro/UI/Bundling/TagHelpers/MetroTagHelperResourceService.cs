using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;

namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public abstract class MetroTagHelperResourceService : ITransientDependency
{
    public ILogger<MetroTagHelperResourceService> Logger { get; set; }
    protected IBundleManager BundleManager { get; }
    protected IWebHostEnvironment HostingEnvironment { get; }
    protected AbpBundlingOptions Options { get; }

    protected MetroTagHelperResourceService(
        IBundleManager bundleManager,
        IOptions<AbpBundlingOptions> options,
        IWebHostEnvironment hostingEnvironment)
    {
        BundleManager = bundleManager;
        HostingEnvironment = hostingEnvironment;
        Options = options.Value;

        Logger = NullLogger<MetroTagHelperResourceService>.Instance;
    }

    public virtual async Task ProcessAsync(
        [NotNull] ViewContext viewContext,
        [NotNull] TagHelper tagHelper,
        [NotNull] TagHelperContext context,
        [NotNull] TagHelperOutput output,
        [NotNull] List<BundleTagHelperItem> bundleItems,
        string? bundleName = null)
    {
        Check.NotNull(viewContext, nameof(viewContext));
        Check.NotNull(context, nameof(context));
        Check.NotNull(output, nameof(output));
        Check.NotNull(bundleItems, nameof(bundleItems));

        var stopwatch = Stopwatch.StartNew();

        output.TagName = null;

        if (bundleName.IsNullOrEmpty())
        {
            bundleName = GenerateBundleName(bundleItems);
        }

        CreateBundle(bundleName, bundleItems);

        var bundleFiles = await GetBundleFilesAsync(bundleName);

        output.Content.Clear();

        foreach (var bundleFile in bundleFiles)
        {
            Logger.LogError($"bundle file name:{bundleFile}");
            var file = HostingEnvironment.WebRootFileProvider.GetFileInfo(bundleFile);

            if (file is not { Exists: true })
            {
                throw new AbpException($"Could not find the bundle file '{bundleFile}' for the bundle '{bundleName}'!");
            }

            if (file.Length > 0)
            {
                AddHtmlTag(viewContext, tagHelper, context, output, bundleFile + "?_v=" + file.LastModified.UtcTicks);
            }
        }

        stopwatch.Stop();
        Logger.LogDebug($"Added bundle '{bundleName}' to the page in {stopwatch.Elapsed.TotalMilliseconds:0.00} ms.");
    }

    protected abstract void CreateBundle(string? bundleName, List<BundleTagHelperItem> bundleItems);

    protected abstract Task<IReadOnlyList<string>> GetBundleFilesAsync(string? bundleName);

    protected abstract void AddHtmlTag(ViewContext viewContext, TagHelper tagHelper, TagHelperContext context, TagHelperOutput output, string file);

    protected virtual string GenerateBundleName(List<BundleTagHelperItem> bundleItems)
    {
        return bundleItems.JoinAsString("|").ToMd5();
    }
}
