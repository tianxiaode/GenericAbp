namespace Generic.Abp.Metro.UI.Bundling;

public interface IBundler
{
    string FileExtension { get; }

    BundleResult Bundle(IBundlerContext context);
}
