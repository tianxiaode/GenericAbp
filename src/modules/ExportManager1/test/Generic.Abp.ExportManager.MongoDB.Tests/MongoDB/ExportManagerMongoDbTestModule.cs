using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Generic.Abp.ExportManager.MongoDB;

[DependsOn(
    typeof(ExportManagerApplicationTestModule),
    typeof(ExportManagerMongoDbModule)
)]
public class ExportManagerMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
