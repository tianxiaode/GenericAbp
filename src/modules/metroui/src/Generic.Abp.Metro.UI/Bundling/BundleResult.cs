namespace Generic.Abp.Metro.UI.Bundling;

public class BundleResult
{
    public string Content { get; }

    public BundleResult(string content)
    {
        Content = content;
    }
}
