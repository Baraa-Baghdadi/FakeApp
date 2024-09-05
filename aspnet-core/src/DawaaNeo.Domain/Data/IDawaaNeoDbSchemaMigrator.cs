using System.Threading.Tasks;

namespace DawaaNeo.Data;

public interface IDawaaNeoDbSchemaMigrator
{
    Task MigrateAsync();
}
