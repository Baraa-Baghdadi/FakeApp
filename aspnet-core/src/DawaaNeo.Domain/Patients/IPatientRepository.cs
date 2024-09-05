using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Patients
{
    public interface IPatientRepository : IRepository<Patient,Guid>
    {
        Task<List<Patient>> GetListAsync(
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

        Task<Patient> GetPatientByMobileNumber(string fullMobileNumber);
    }
}
