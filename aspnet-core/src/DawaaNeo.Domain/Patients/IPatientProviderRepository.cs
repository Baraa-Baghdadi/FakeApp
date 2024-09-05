using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Patients
{
    public interface IPatientProviderRepository : IRepository<PatientProvider,Guid>
    {

        // Method For Portal => Tenant should be enable
        Task<List<PatientProvider>> GetListAsync(
        string? filterText = null,
        string? mobileNumber = null,
        string? countryCode = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
        string? filterText = null,
        string? mobileNumber = null,
        string? countryCode = null,
        CancellationToken cancellationToken = default
        );

        Task<PatientProvider> GetAsync(Guid id);


        // Methods for mobile => Tenant should be disable
        Task<List<PatientProvider>> GetListForMobileAsync(
        string? userPhoneNumber = null,
        string? filterText = null,
        string? pharmacyName = null,
        string? pharmacyPhone = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
        );

        Task<PatientProvider> GetProviderForMobileAsync(Guid id, string? userPhoneNumber = null);

        Task<long> GetCountForMobileAsync(
        string? userPhoneNumber = null,
        string? filterText = null,
        string? pharmacyName = null,
        string? pharmacyPhone = null,
        CancellationToken cancellationToken  = default
        );
    }
}
