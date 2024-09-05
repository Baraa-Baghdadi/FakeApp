using DawaaNeo.EntityFrameworkCore;
using DawaaNeo.Patients;
using DawaaNeo.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using DawaaNeo.Service;

namespace DawaaNeo.Services
{
    public class EfCoreServiceRepository : EfCoreRepository<DawaaNeoDbContext, Service, Guid>, IServiceRepository
    {
        public EfCoreServiceRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> GetCountAsync(string? filterText = null, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Service>> GetListAsync(string? filterText = null, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ServiceConst.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual IQueryable<Service> ApplyFilter(
        IQueryable<Service> query,
        string? filterText = null
    )
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Title!.Contains(filterText!) || e.ArTitle!.Contains(filterText!));
        }
    }
}
