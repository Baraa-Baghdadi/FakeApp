using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace DawaaNeo.Data;

/* This is used if database provider does't define
 * IDawaaNeoDbSchemaMigrator implementation.
 */
public class NullDawaaNeoDbSchemaMigrator : IDawaaNeoDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
