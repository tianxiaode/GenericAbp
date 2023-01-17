using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Abp.Metro.UI.Bundling;

public interface IBundleManager
{
    Task<IReadOnlyList<string>> GetStyleBundleFilesAsync(string bundleName);

    Task<IReadOnlyList<string>> GetScriptBundleFilesAsync(string bundleName);
}
