namespace Generic.Abp.Extensions.Entities.SearchParams;

public class SearchParams : ISearchParams
{
    public string? Filter { get; set; } = default!;
}