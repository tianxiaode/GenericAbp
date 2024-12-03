using System;

namespace Generic.Abp.FileManagement.ExternalShares.Dtos;

[Serializable]
public class ExternalShareTokenDto(string token)
{
    public string Token { get; set; } = token;
}