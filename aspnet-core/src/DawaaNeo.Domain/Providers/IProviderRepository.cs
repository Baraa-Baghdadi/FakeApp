using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Providers
{
    public interface IProviderRepository : IRepository<Provider, Guid>
    {
        Task<List<Provider>> GetListAsync(
        string? filterText = null,
        string? email = null,
        string? pharmacyName = null,
        string? pharmacyPhone = null,
        string? sorting = null, 
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
        string? filterText = null,
        string? email = null,
        string? pharmacyName = null,
        string? pharmacyPhone = null,
        CancellationToken cancellationToken = default
        );

        Task<Provider> GetWithWorkTimesAsync(string providerId);

        Task<Provider> GetProviderByEmail(string email);
    }
}
