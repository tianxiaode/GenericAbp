using System;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

[Serializable]
public class GetExternalShareResourcesDto
{
    public Guid? ResourceId { get; set; }
    public string Token { get; set; } = default!;
}