using DawaaNeo.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Patients
{
    public class PatientAddressDto : EntityDto<Guid>
    {
        public virtual Guid PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string BuildingName { get; set; } = null!;
        public string AppartmentNumber { get; set; } = null!;
        public string LandMark { get; set; } = null!;
        public string Longitude { get; set; } = null!;
        public string Latitude { get; set; } = null!;
        public string Address { get; set; } = null!;
        public PatientAddressType Type { get; set; }
        public bool IsDefault { get; set; }

    }
}
