namespace Generic.Abp.Metro.UI.Bundling;

public interface IBundlerContext
{
    string BundleRelativePath { get; }

    IReadOnlyList<string> ContentFiles { get; }

    bool IsMinificationEnabled { get; }
}
