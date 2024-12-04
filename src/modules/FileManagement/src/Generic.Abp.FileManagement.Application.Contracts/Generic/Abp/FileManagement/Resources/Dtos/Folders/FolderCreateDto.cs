using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Generic.Abp.FileManagement.Resources.Dtos.Folders;

[Serializable]
public class FolderCreateDto : FolderCreateOrUpdateDto
{
    [Required]
    [DisplayName("Folder:Parent")]
    public Guid ParentId { get; set; } = default!;
}