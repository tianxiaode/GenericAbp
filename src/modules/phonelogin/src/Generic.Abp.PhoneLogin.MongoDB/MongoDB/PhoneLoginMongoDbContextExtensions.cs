using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Generic.Abp.PhoneLogin.MongoDB;

public static class PhoneLoginMongoDbContextExtensions
{
    public static void ConfigurePhoneLogin(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
