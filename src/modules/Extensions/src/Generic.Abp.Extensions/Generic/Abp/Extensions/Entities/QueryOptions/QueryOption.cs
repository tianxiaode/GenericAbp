namespace Generic.Abp.Extensions.Entities.QueryOptions;

public class QueryOption
{
    public string Sorting { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }

    public QueryOption(string? sorting = null, int skipCount = 0, int maxResultCount = int.MaxValue,
        bool withEntityName = false)
    {
        Sorting = sorting ?? GetDefaultSorting(withEntityName);
        SkipCount = skipCount;
        MaxResultCount = maxResultCount;
    }

    // 虚属性，允许子类提供自己的默认排序模式
    protected virtual string DefaultSortingPattern => "{0}Name asc";

    // 虚属性，允许子类提供自己的实体名称前缀
    protected virtual string EntityNamePrefix => string.Empty;

    protected string GetDefaultSorting(bool withEntityName = false)
    {
        return string.Format(DefaultSortingPattern, withEntityName ? EntityNamePrefix : string.Empty);
    }
}