namespace Generic.Abp.Extensions.Entities.GetListParams;

public class SearchParams : ISearchParams
{
    public string? Filter { get; set; } = default!;
}