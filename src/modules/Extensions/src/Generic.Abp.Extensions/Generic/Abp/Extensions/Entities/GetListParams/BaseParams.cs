namespace Generic.Abp.Extensions.Entities.GetListParams;

public class BaseParams : IHasFilter
{
    public string? Filter { get; set; } = default!;
    public string? Sorting { get; set; } = default!;
    public int MaxResultCount { get; set; } = default!;
    public int SkipCount { get; set; } = default!;
}