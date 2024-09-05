using DawaaNeo;
using DawaaNeo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DawaaNeo.Providers
{
    public class EfCoreProviderRepository : EfCoreRepository<DawaaNeoDbContext, Provider, Guid>, IProviderRepository
    {
        public EfCoreProviderRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
            
        }
        public async Task<long> GetCountAsync(
            string? filterText = null, string email = null, 
            string? pharmacyName = null, 
            string? pharmacyPhone = null, 
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, pharmacyName, pharmacyPhone);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
   
        }

        public async Task<List<Provider>> GetListAsync(
            string? filterText = null,
            string? email = null, 
            string? pharmacyName = null, 
            string? pharmacyPhone = null, 
            string? sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, pharmacyName, pharmacyPhone);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProviderConst.GetDeafultSorting(false) : sorting);
            return await query.PageBy(skipCount,maxResultCount).ToListAsync(cancellationToken);
            
        }

        public async Task<Provider> GetProviderByEmail(string email)
        {
            var query = (await GetQueryableAsync()).Where(p => p.Email == email);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Provider> GetWithWorkTimesAsync(string providerId)
        {
            var query = (await GetQueryableAsync()).Where(p => p.Id == new Guid(providerId)).Include("WorkingTimes");
            return await query.FirstAsync();
        }

        protected virtual IQueryable<Provider> ApplyFilter(
            IQueryable<Provider> query,
            string? fillterText = null,
            string? email = null,
            string? pharmacyName = null,
            string? pharmacyPhone = null          
            )
        {
            return query.WhereIf(!string.IsNullOrWhiteSpace(fillterText), e => e.Email!.Contains(fillterText!) || e.PharmacyName!.Contains(fillterText!) || e.PharmacyPhone!.Contains(fillterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(email), e => e.Email!.Contains(email!))
                    .WhereIf(!string.IsNullOrWhiteSpace(pharmacyName), e => e.Email!.Contains(pharmacyName!))
                    .WhereIf(!string.IsNullOrWhiteSpace(pharmacyPhone), e => e.Email!.Contains(pharmacyPhone!));
        }
    }
}
