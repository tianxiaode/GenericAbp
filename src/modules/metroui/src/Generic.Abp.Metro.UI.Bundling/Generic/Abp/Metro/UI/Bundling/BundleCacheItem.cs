using System;
using System.Collections.Generic;

namespace Generic.Abp.Metro.UI.Bundling;

public class BundleCacheItem
{
    public List<string> Files { get; }

    public List<IDisposable> WatchDisposeHandles { get; }

    public BundleCacheItem(List<string> files)
    {
        Files = files;
        WatchDisposeHandles = new List<IDisposable>();
    }
}
