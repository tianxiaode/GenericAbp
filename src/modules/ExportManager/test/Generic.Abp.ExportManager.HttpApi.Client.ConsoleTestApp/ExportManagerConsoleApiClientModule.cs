using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Generic.Abp.ExportManager;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ExportManagerHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class ExportManagerConsoleApiClientModule : AbpModule
{

}
