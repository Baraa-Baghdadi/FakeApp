using DawaaNeo.Data;
using DawaaNeo.OpenIddict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace DawaaNeo.DataSeeder
{
    public class SeederService : ITransientDependency
    {
        private readonly DawaaNeoDbMigrationService _migrationService;
        private readonly DawaaNeoSeedContributer _seedContributer;
        public SeederService( DawaaNeoSeedContributer seedContributer, DawaaNeoDbMigrationService migrationService)
        {
            _seedContributer = seedContributer;
            _migrationService = migrationService;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync ()
        {
            await _migrationService.MigrateAsync();
            // await _seedContributer.Seed();
        }
    }
}
