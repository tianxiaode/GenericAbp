using System;
using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling;

public class BundleCacheItem
{
    public List<BundleFile> Files { get; }

    public List<IDisposable> WatchDisposeHandles { get; }

    public BundleCacheItem(List<BundleFile> files)
    {
        Files = files;
        WatchDisposeHandles = new List<IDisposable>();
    }
}