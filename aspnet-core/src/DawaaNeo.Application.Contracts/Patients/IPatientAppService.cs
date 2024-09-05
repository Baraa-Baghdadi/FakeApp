using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Patients
{
    public interface IPatientAppService
    {
        Task<PagedResultDto<PatientProviderDto>> GetAllPatientsOfProviderAsync(GetPatientInput input);
        Task<PatientProviderDto> GetPatientsOfProviderAsync(Guid id);
    }
}
