namespace Generic.Abp.ExternalAuthentication.Models;

[Serializable]
public class ExternalProviderGetListInput
{
    public bool OnlyEnabled { get; set; } = false;
}