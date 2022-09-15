using Volo.Abp.Application.Dtos;

namespace Generic.Abp.FileManagement.Files;

public class FileGetListInput: PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}