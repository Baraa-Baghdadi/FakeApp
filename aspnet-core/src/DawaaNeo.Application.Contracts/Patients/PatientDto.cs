using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace DawaaNeo.Patients
{
    public class PatientDto : FullAuditedEntityDto<Guid>,IHasConcurrencyStamp
    {
        public string MobileNumber { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        public string ConcurrencyStamp { get; set; } = null!;
        public List<PatientAddressDto> PatientAddresses { get; set; }
        public List<PatientProviderDto> PatientProviders { get; set; }
    }
}
