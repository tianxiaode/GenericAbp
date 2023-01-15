using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.Theme.Shared;

public class AbpErrorPageOptions
{
    public readonly IDictionary<string, string> ErrorViewUrls;

    public AbpErrorPageOptions()
    {
        ErrorViewUrls = new Dictionary<string, string>();
    }
}
