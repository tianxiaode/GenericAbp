namespace Generic.Abp.IdentityServer.ApiResources;

public class ApiResourceUpdateInput: ApiResourceCreateOrUpdateInput
{
    public string ConcurrencyStamp { get; set; }
}