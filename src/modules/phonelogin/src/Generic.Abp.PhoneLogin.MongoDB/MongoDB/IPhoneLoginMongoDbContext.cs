using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Generic.Abp.PhoneLogin.MongoDB;

[ConnectionStringName(PhoneLoginDbProperties.ConnectionStringName)]
public interface IPhoneLoginMongoDbContext : IAbpMongoDbContext
{
    /* Define mongo collections here. Example:
     * IMongoCollection<Question> Questions { get; }
     */
}
