using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DawaaNeo.Data;
using Volo.Abp.DependencyInjection;

namespace DawaaNeo.EntityFrameworkCore;

public class EntityFrameworkCoreDawaaNeoDbSchemaMigrator
    : IDawaaNeoDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreDawaaNeoDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the DawaaNeoDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<DawaaNeoDbContext>()
            .Database
            .MigrateAsync();
    }
}
