namespace Generic.Abp.Extensions.Entities.QueryParams;

public interface IBaseQueryParams
{
    string? Sorting { get; set; }
    int MaxResultCount { get; set; }
    int SkipCount { get; set; }
    string? Filter { get; set; }
}