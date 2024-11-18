using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Generic.Abp.FileManagement.Folders.Dtos;

[Serializable]
public class FolderCreateDto : FolderCreateOrUpdateDto
{
    [Required]
    [DisplayName("Folder:Parent")]
    public Guid ParentId { get; set; } = default!;
}