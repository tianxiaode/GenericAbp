using Generic.Abp.Extensions.Entities.QueryParams;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.UserFolders.Dtos;

public class UserGetListInput : PagedAndSortedResultRequestDto, IHasFilter
{
    public string? Filter { get; set; }
}