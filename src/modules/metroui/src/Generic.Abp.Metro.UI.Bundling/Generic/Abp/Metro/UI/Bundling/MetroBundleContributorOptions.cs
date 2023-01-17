using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Generic.Abp.Metro.UI.Bundling;

public class MetroBundleContributorOptions
{
    public ConcurrentDictionary<Type, BundleContributorCollection> AllExtensions { get; }

    public MetroBundleContributorOptions()
    {
        AllExtensions = new ConcurrentDictionary<Type, BundleContributorCollection>();
    }

    public BundleContributorCollection Extensions<TContributor>()
    {
        return Extensions(typeof(TContributor));
    }

    public BundleContributorCollection Extensions([NotNull] Type contributorType)
    {
        Check.NotNull(contributorType, nameof(contributorType));

        return AllExtensions.GetOrAdd(
            contributorType,
            _ => new BundleContributorCollection()
        );
    }
}
