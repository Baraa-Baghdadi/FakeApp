using DawaaNeo.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Services
{
    public interface IServiceRepository : IRepository<Service,Guid>
    {
        Task<List<Service>> GetListAsync(
        string? filterText = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
        );
        Task<long> GetCountAsync(
        string? filterText = null,
        CancellationToken cancellationToken = default
        );
    }
}
