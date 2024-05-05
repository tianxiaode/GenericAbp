using System.Collections.Generic;

namespace Generic.Abp.TailWindCss.Account.Web;

public class TailWindErrorPageOptions
{
    public readonly IDictionary<string, string> ErrorViewUrls;

    public TailWindErrorPageOptions()
    {
        ErrorViewUrls = new Dictionary<string, string>();
    }
}