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

namespace DawaaNeo.Patients
{
    public class EfCorePatientRepository : EfCoreRepository<DawaaNeoDbContext, Patient, Guid>, IPatientRepository
    {
        public EfCorePatientRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<long> GetCountAsync(
            string? filterText = null, 
            string? mobileNumber = null, 
            string? countryCode = null, 
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, mobileNumber, countryCode);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<Patient>> GetListAsync(
            string? filterText = null, 
            string? mobileNumber = null, 
            string? countryCode = null, 
            string? sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, mobileNumber, countryCode);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PatientConst.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<Patient> GetPatientByMobileNumber(string fullMobileNumber)
        {
            var getPatientId = (await GetQueryableAsync()).Select(row => new
            {
                PatientId = row.Id,
                FullMobileNumber = row.CountryCode + row.MobileNumber
            }).Where(p => p.FullMobileNumber == fullMobileNumber).AsQueryable().FirstOrDefaultAsync();
            var patient = (await GetQueryableAsync()).Where(p => p.Id == getPatientId.Result.PatientId);
            return await patient.FirstOrDefaultAsync();
        }

        protected virtual IQueryable<Patient> ApplyFilter(
            IQueryable<Patient> query,
            string? filterText = null,
            string? mobileNumber = null,
            string? countryCode = null
            )
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.MobileNumber!.Contains(filterText!) || e.CountryCode!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(mobileNumber), e => e.MobileNumber!.Contains(mobileNumber!))
                .WhereIf(!string.IsNullOrWhiteSpace(countryCode), e => e.CountryCode!.Contains(countryCode!));
        }
    }
}
