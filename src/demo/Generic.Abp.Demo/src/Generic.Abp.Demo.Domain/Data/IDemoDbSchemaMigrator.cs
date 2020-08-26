using System.Threading.Tasks;

namespace Generic.Abp.Demo.Data
{
    public interface IDemoDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
