using DawaaNeo.Data;
using DawaaNeo.OpenIddict;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace DawaaNeo.DataSeeder
{
    public class SeederService : ITransientDependency
    {
        private readonly DawaaNeoDbMigrationService _migrationService;
        private readonly DawaaNeoSeedContributer _seedContributer;
        private readonly OpenIddictDataSeedContributor _openIddictSeedContributer;
        public SeederService( DawaaNeoSeedContributer seedContributer, DawaaNeoDbMigrationService migrationService, OpenIddictDataSeedContributor openIddictSeedContributer)
        {
            _seedContributer = seedContributer;
            _migrationService = migrationService;
            _openIddictSeedContributer = openIddictSeedContributer;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync ()
        {
            await _migrationService.MigrateAsync();
            // await _seedContributer.Seed();
            var x = new DataSeedContext();
            await _openIddictSeedContributer.SeedAsync(x);
        }
    }
}
