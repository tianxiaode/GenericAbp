namespace Generic.Abp.ExternalAuthentication.dtos;

[Serializable]
public class ExternalSettingDto
{
    public string NewUserPrefix { get; set; } = string.Empty;
    public string NewUserEmailSuffix { get; set; } = string.Empty;

    public List<ExternalProviderDto> Providers { get; set; } = [];
}