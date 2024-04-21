using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Generic.Abp.ExportManager.MongoDB;

public static class ExportManagerMongoDbContextExtensions
{
    public static void ConfigureExportManager(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
