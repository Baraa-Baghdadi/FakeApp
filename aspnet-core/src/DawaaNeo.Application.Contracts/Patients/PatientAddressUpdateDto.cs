using DawaaNeo.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Patients
{
    public class PatientAddressUpdateDto : EntityDto<Guid>
    {
        public virtual string? Name { get; set; }
        public virtual string? BuildingName { get; set; }
        public virtual string? AppartmentNumber { get; set; }
        public virtual string? LandMark { get; set; }
        public virtual string? Longitude { get; set; }
        public virtual string? Latitude { get; set; }
        public virtual string? Address { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual PatientAddressType? Type { get; set; }
    }
}
