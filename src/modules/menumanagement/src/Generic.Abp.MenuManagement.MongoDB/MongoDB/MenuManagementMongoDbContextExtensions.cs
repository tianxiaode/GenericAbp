using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Generic.Abp.MenuManagement.MongoDB;

public static class MenuManagementMongoDbContextExtensions
{
    public static void ConfigureMenuManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
