using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.Resources;

public interface IWebRequestResources
{
    List<string> TryAdd(List<string> resources);
}
