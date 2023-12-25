using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Resources;

public interface IWebRequestResources
{
    List<BundleFile> TryAdd(List<BundleFile> resources);
}