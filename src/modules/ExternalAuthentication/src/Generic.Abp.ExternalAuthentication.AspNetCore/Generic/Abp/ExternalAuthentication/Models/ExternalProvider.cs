namespace Generic.Abp.ExternalAuthentication.Models;

[Serializable]
public class ExternalProvider
{
    public string Provider { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
}