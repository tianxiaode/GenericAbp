using Volo.Abp.Application.Dtos;

namespace Generic.Abp.OpenIddict.Scopes
{
    [Serializable]
    public class ScopeGetListInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; } = default!;
    }
}