using System;

namespace Generic.Abp.Extensions.Entities.QueryParams;

public class BaseQueryParams : IBaseQueryParams
{
    public string Sorting { get; set; }
    public int MaxResultCount { get; set; }
    public int SkipCount { get; set; }
    public string? Filter { get; set; } = default!;

    public BaseQueryParams(string? sorting = null, int skipCount = 0, int maxResultCount = int.MaxValue,
        bool withEntityName = false)
    {
        Sorting = !string.IsNullOrWhiteSpace(sorting)
            ? sorting
            : GetDefaultSorting(withEntityName);
        SkipCount = skipCount;
        MaxResultCount = maxResultCount;
    }

    // 虚属性，允许子类提供自己的默认排序模式
    protected virtual string DefaultSortingPattern => "{0}Id";

    // 虚属性，允许子类提供自己的实体名称前缀
    protected virtual string EntityNamePrefix => string.Empty;

    protected string GetDefaultSorting(bool withEntityName = false)
    {
        var sorting = string.IsNullOrEmpty(DefaultSortingPattern)
            ? string.Empty
            : string.Format(DefaultSortingPattern, withEntityName ? EntityNamePrefix : string.Empty);
        if (string.IsNullOrWhiteSpace(sorting))
        {
            throw new InvalidOperationException(
                "The default sorting pattern is empty or not defined in the current query parameter configuration.");
        }

        return sorting;
    }
}