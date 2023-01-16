using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.Theme.Shared;

public class MetroErrorPageOptions
{
    public readonly IDictionary<string, string> ErrorViewUrls;

    public MetroErrorPageOptions()
    {
        ErrorViewUrls = new Dictionary<string, string>();
    }
}
