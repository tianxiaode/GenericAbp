using Volo.Abp.Application.Dtos;

namespace Generic.Abp.OpenIddict.Applications
{
    [Serializable]
    public class ApplicationGetListInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; } = default!;
    }
}