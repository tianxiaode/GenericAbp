using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Abp.Extensions.Entities.QueryParams;

[Serializable]
public abstract class BaseQueryParams : IBaseQueryParams
{
    public virtual string? Sorting { get; set; } = default!;
    public virtual int MaxResultCount { get; set; }
    public virtual int SkipCount { get; set; }
    public virtual string? Filter { get; set; } = default!;

    protected abstract HashSet<string> AllowedSortingFields { get; set; }
    protected virtual string DefaultSortingPattern => "{0}{1} {2}";
    protected virtual string DefaultSortingOrder => "asc";
    protected virtual string EntityNamePrefix => string.Empty;

    public string GetSorting(bool withEntityName = false)
    {
        var field = AllowedSortingFields.FirstOrDefault();
        var order = DefaultSortingOrder;

        if (string.IsNullOrEmpty(field))
        {
            throw new InvalidOperationException("AllowedSortingFields is not set.");
        }

        if (Sorting.IsNullOrEmpty())
        {
            return string.Format(DefaultSortingPattern, withEntityName ? EntityNamePrefix : string.Empty, field, order);
        }

        field = AllowedSortingFields.FirstOrDefault(m =>
            Sorting.StartsWith(m, StringComparison.InvariantCultureIgnoreCase));
        if (string.IsNullOrEmpty(field))
        {
            throw new InvalidOperationException($"Sorting field '{Sorting}' is not allowed.");
        }
#if NETSTANDARD2_0
        order = Sorting.ToLower().EndsWith("desc") ? "desc" : "asc";
#else
        order = Sorting.Contains("desc", StringComparison.InvariantCultureIgnoreCase) ? "desc" : "asc";
#endif
        return string.Format(DefaultSortingPattern, withEntityName ? EntityNamePrefix : string.Empty, field, order);
    }
}