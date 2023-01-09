using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Generic.Abp.PhoneLogin.MongoDB;

[ConnectionStringName(PhoneLoginDbProperties.ConnectionStringName)]
public class PhoneLoginMongoDbContext : AbpMongoDbContext, IPhoneLoginMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigurePhoneLogin();
    }
}
