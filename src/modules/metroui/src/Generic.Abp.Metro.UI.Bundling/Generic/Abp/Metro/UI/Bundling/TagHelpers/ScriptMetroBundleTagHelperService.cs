namespace Generic.Abp.Metro.UI.Bundling.TagHelpers;

public class ScriptMetroBundleTagHelperService : MetroBundleTagHelperService<ScriptMetroBundleTagHelper, ScriptMetroBundleTagHelperService>
{
    public ScriptMetroBundleTagHelperService(MetroTagHelperScriptService resourceHelper)
        : base(resourceHelper)
    {
    }
}
