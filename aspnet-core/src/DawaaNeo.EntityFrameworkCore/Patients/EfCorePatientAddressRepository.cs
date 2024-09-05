using DawaaNeo.EntityFrameworkCore;
using DawaaNeo.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace DawaaNeo.Patients
{
    public class EfCorePatientAddressRepository : EfCoreRepository<DawaaNeoDbContext, PatientAddress, Guid>, IPatientAddressRepository
    {
        public EfCorePatientAddressRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<(List<PatientAddress>, int)> GetUserAddresses(
            Guid patientId, string? name,
            string? appartmentNumber, string? buildingName, 
            string longitude, string? latitude, 
            string? landmark, string? address,
            PatientAddressType? type, int skipCount, 
            int maxResultCount, string? sorting
            )
        {
            var addresses = (await GetQueryableAsync()).Where(a => a.PatientId == patientId);
            addresses = ApplyFilter(addresses, name, appartmentNumber, buildingName, longitude, latitude, landmark, address, type);
            addresses = addresses.OrderBy(string.IsNullOrEmpty(sorting) ? PatientAddressConsts.GetDefaultSorting(false) : sorting);
            var numberOfRecord = addresses.Count();
            var entites = addresses.PageBy(skipCount, maxResultCount).ToList();
            return new(entites, numberOfRecord);
        }
        protected virtual IQueryable<PatientAddress> ApplyFilter(
          IQueryable<PatientAddress> query,
            string? name,
            string? appartmentNumber, string? buildingName,
            string longitude, string? latitude,
            string? landmark, string? address,
            PatientAddressType? type
        )
        {
            return query.Where(a =>
            (string.IsNullOrEmpty(name) || a.Name!.Contains(name)) &&
            (string.IsNullOrEmpty(appartmentNumber) || a.AppartmentNumber!.Contains(appartmentNumber)) &&
            (string.IsNullOrEmpty(buildingName) || a.BuildingName!.Contains(buildingName)) &&
            (string.IsNullOrEmpty(longitude) || a.Longitude!.Contains(longitude)) &&
            (string.IsNullOrEmpty(latitude) || a.Latitude!.Contains(latitude)) &&
            (string.IsNullOrEmpty(landmark) || a.LandMark!.Contains(landmark)) &&
            (string.IsNullOrEmpty(address) || a.Address!.Contains(address)) &&
            (type == null || a.Type == type)
            );
        }
    }
}
