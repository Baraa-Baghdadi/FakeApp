using DawaaNeo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace DawaaNeo.Patients
{
    public class EfCorePatientProviderRepository : EfCoreRepository<DawaaNeoDbContext, PatientProvider, Guid>, IPatientProviderRepository
    {
        private readonly IDataFilter _dataFilter;
        public EfCorePatientProviderRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider , IDataFilter dataFilter) : base(dbContextProvider)
        {
            _dataFilter = dataFilter;
        }

        #region Portal Methods
        public async Task<List<PatientProvider>> GetListAsync(
            string? filterText = null, 
            string? mobileNumber = null, 
            string? countryCode = null, 
            string? sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            CancellationToken cancellationToken = default)
        {
            var query = (await GetQueryableAsync()).Include("Patient").Include("Provider");
            query = ApplyFilterForPortal(query, filterText, mobileNumber, countryCode);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PatientProviderConst.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<PatientProvider> GetAsync(Guid id)
        {
            var query = (await GetQueryableAsync()).Include("Patient");
            return await query.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<long> GetCountAsync(string? filterText = null, string? mobileNumber = null, string? countryCode = null, CancellationToken cancellationToken = default)
        {
        using (_dataFilter.Enable<IMultiTenant>())
        {
          var query = (await GetQueryableAsync()).Include("Patient").Include("Provider");
          query = ApplyFilterForPortal(query, filterText, mobileNumber, countryCode);
          return (await query.ToListAsync()).LongCount();
        }
        }

        protected virtual IQueryable<PatientProvider> ApplyFilterForPortal(
          IQueryable<PatientProvider> query,  
          string? filterText = null,
          string? mobileNumber = null,
          string? countryCode = null
        )
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Patient.MobileNumber!.Contains(filterText!) || e.Patient.CountryCode!.Contains(filterText!) || e.Patient.FullMobileNumber!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(mobileNumber), e => e.Patient.MobileNumber!.Contains(mobileNumber!))
                .WhereIf(!string.IsNullOrWhiteSpace(countryCode), e => e.Patient.CountryCode!.Contains(countryCode!));
        }
        #endregion

        #region Mobile Methods

        public async Task<long> GetCountForMobileAsync(
        string? userPhoneNumber = null,
        string? filterText = null,
        string? pharmacyName = null,
        string? pharmacyPhone = null,
        CancellationToken cancellationToken = default)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var query = (await GetQueryableAsync()).Include("Provider").Include("Patient")
                    .Where(p => p.Patient.CountryCode + p.Patient.MobileNumber == userPhoneNumber);
                query = ApplyFilterForMobile(query, filterText, pharmacyName, pharmacyPhone);
                return await query.LongCountAsync(GetCancellationToken(cancellationToken));
            }
        }

        public async Task<List<PatientProvider>> GetListForMobileAsync(
            string? userPhoneNumber = null, 
            string? filterText = null, 
            string? pharmacyName = null, 
            string? pharmacyPhone = null, 
            string? sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            CancellationToken cancellationToken = default)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var query = (await GetQueryableAsync()).IgnoreQueryFilters();
                query = query.Where(p => p.Patient!.FullMobileNumber == userPhoneNumber);
                query = ApplyFilterForMobile(query, filterText, pharmacyName, pharmacyPhone);
                query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PatientProviderConst.GetDefaultSorting(false) : sorting);
                return await query.PageBy(skipCount,maxResultCount).ToListAsync(cancellationToken);
            }
        }

        public async Task<PatientProvider> GetProviderForMobileAsync(Guid id, string? userPhoneNumber = null)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var query = (await GetQueryableAsync()).Include(p => p.Provider)
                    .ThenInclude(p => p.WorkingTimes)
                    .Where(p => p.Patient.CountryCode + p.Patient.MobileNumber == userPhoneNumber).AsQueryable();
                return await query.FirstOrDefaultAsync(p => p.Provider.Id == id);
            }
        }

        protected virtual IQueryable<PatientProvider> ApplyFilterForMobile(
          IQueryable<PatientProvider> query,
          string? filterText = null,
          string? pharmacyName = null,
          string? pharmacyPhone = null
        )
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Provider.PharmacyName!.Contains(filterText!) || e.Provider.PharmacyPhone!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(pharmacyName), e => e.Provider.PharmacyName!.Contains(pharmacyName!))
                .WhereIf(!string.IsNullOrWhiteSpace(pharmacyPhone), e => e.Provider.PharmacyPhone!.Contains(pharmacyPhone!));
        }

        #endregion
    }
}
