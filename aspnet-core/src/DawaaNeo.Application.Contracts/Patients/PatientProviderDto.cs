using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Patients
{
    public class PatientProviderDto : EntityDto<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid ProviderId { get; set; }
        public DateTime AddingDate { get; set; }
        public string? MobileNumber { get; set; }
        public string? CountryCode { get; set; }
        public PatientAddingType AddingType { get; set; }
    }
}
