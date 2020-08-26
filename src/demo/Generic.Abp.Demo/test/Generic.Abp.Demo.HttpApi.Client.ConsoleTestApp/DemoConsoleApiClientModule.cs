using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Generic.Abp.Demo.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(DemoHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class DemoConsoleApiClientModule : AbpModule
    {
        
    }
}
