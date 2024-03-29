#nullable enable
using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Minify;

namespace Generic.Abp.Metro.UI.Bundling;

public abstract class BundlerBase : IBundler, ITransientDependency
{
    private static readonly string[] MinFileSuffixes = { "min", "prod" };

    public ILogger<BundlerBase> Logger { get; set; }

    protected IWebHostEnvironment HostEnvironment { get; }
    protected IMinifier Minifier { get; }
    protected AbpBundlingOptions MetroBundlingOptions { get; }

    protected BundlerBase(
        IWebHostEnvironment hostEnvironment,
        IMinifier minifier,
        IOptions<AbpBundlingOptions> bundlingOptions)
    {
        HostEnvironment = hostEnvironment;
        Minifier = minifier;
        MetroBundlingOptions = bundlingOptions.Value;

        Logger = NullLogger<BundlerBase>.Instance;
    }

    public abstract string FileExtension { get; }

    public BundleResult Bundle(IBundlerContext context)
    {
        Logger.LogInformation($"Bundling {context.BundleRelativePath} ({context.ContentFiles.Count} files)");

        var bundleContentBuilder = new StringBuilder();

        Logger.LogDebug("Bundle files:");
        foreach (var file in context.ContentFiles)
        {
            AddFileToBundle(context, bundleContentBuilder, file);
        }

        var bundleContent = bundleContentBuilder.ToString();
        Logger.LogInformation($"Bundled {context.BundleRelativePath} ({bundleContent.Length} bytes)");

        return new BundleResult(bundleContent);
    }

    private void AddFileToBundle(IBundlerContext context, StringBuilder bundleContentBuilder, string fileName)
    {
        var fileContent = GetFileContentConsideringMinification(context, fileName);
        fileContent = ProcessBeforeAddingToTheBundle(context, fileName, fileContent);
        bundleContentBuilder.Append(fileContent);
    }

    private string GetFileContentConsideringMinification(IBundlerContext context, string fileName)
    {
        var isIgnoredForMinification = MetroBundlingOptions.MinificationIgnoredFiles.Contains(fileName);
        var isMinFile = IsMinFile(fileName);
        if (!context.IsMinificationEnabled || isIgnoredForMinification || isMinFile)
        {
            var fileContent = GetFileInfo(context, fileName).ReadAsString();
            Logger.LogDebug($"- {fileName} ({fileContent.Length} bytes)");
            if (!context.IsMinificationEnabled) return fileContent;
            if (isMinFile)
            {
                Logger.LogDebug("  > Already minified.");
            }
            else if (isIgnoredForMinification)
            {
                Logger.LogDebug("  > Ignored for minification.");
            }

            return fileContent;
        }

        var minFileInfo = GetMinFileInfoOrNull(fileName);
        if (minFileInfo == null) return GetAndMinifyFileContent(context, fileName);
        {
            var fileContent = minFileInfo.ReadAsString();
            Logger.LogDebug($"- {fileName}");
            Logger.LogDebug($"  > Using the pre-minified file: {minFileInfo.Name} ({fileContent.Length} bytes)");
            return fileContent;
        }
    }

    private string GetAndMinifyFileContent(IBundlerContext context, string fileName)
    {
        var fileContent = GetFileInfo(context, fileName).ReadAsString();
        var nonMinifiedSize = fileContent.Length;

        Logger.LogDebug($"- {fileName} ({nonMinifiedSize} bytes) - non minified, minifying...");

        try
        {
            fileContent = Minifier.Minify(
                fileContent,
                context.BundleRelativePath,
                fileName
            );

            Logger.LogInformation($"  > Minified {fileName} ({nonMinifiedSize} bytes -> {fileContent.Length} bytes)");

            return fileContent;
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"Unable to minify the file: {fileName}. Return file content without minification.", ex);
        }

        return fileContent;
    }

    protected virtual IFileInfo GetFileInfo(IBundlerContext context, string file)
    {
        var fileInfo = HostEnvironment.WebRootFileProvider.GetFileInfo(file);

        if (!fileInfo.Exists)
        {
            throw new AbpException($"Could not find file '{file}'");
        }

        return fileInfo;
    }

    protected virtual bool IsMinFile(string fileName)
    {
        return MinFileSuffixes.Any(suffix =>
            fileName.EndsWith($".{suffix}.{FileExtension}", StringComparison.InvariantCultureIgnoreCase));
    }

    protected virtual IFileInfo? GetMinFileInfoOrNull(string file)
    {
        return MinFileSuffixes
            .Select(suffix =>
                HostEnvironment.WebRootFileProvider.GetFileInfo(
                    $"{file.RemovePostFix($".{FileExtension}")}.{suffix}.{FileExtension}"))
            .FirstOrDefault(fileInfo => fileInfo.Exists);
    }

    protected virtual string ProcessBeforeAddingToTheBundle(IBundlerContext context, string filePath,
        string fileContent)
    {
        return fileContent;
    }
}