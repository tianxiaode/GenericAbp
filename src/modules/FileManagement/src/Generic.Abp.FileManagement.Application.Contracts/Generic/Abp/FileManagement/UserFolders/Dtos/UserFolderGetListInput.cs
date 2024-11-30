using System;
using Generic.Abp.Extensions.Entities.GetListParams;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.UserFolders.Dtos;

[Serializable]
public class UserFolderGetListInput : PagedAndSortedResultRequestDto, IHasFilter, IHasCreationTimeSearch
{
    public string? Filter { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid? OwnerId { get; set; }
}