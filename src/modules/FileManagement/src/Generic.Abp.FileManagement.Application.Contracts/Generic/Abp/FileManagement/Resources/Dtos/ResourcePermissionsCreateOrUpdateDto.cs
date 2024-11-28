using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Resources.Dtos;

public class ResourcePermissionsCreateOrUpdateDto
{
    public List<ResourcePermissionCreateOrUpdateDto> Permissions { get; set; } = default!;
}