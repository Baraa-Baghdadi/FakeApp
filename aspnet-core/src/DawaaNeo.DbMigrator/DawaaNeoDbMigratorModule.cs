using DawaaNeo.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace DawaaNeo.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(DawaaNeoEntityFrameworkCoreModule),
    typeof(DawaaNeoApplicationContractsModule)
    )]
public class DawaaNeoDbMigratorModule : AbpModule
{
}
