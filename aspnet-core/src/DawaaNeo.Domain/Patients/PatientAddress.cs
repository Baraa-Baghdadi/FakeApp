using DawaaNeo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Patients
{
    public class PatientAddress : FullAuditedAggregateRoot<Guid>
    {
        public virtual Guid PatientId { get; set; }
        public virtual bool IsDefault { get; set; }
        public virtual string? Name { get; set; }
        public virtual string? BuildingName { get; set; }
        public virtual string? AppartmentNumber { get; set; }
        public virtual string? LandMark { get; set; }
        public virtual string? Longitude { get; set; }
        public virtual string? Latitude { get; set; }
        public virtual string? Address { get; set; }
        public virtual PatientAddressType Type { get; set; }

        protected PatientAddress()
        {

        }

        public PatientAddress(Guid id,Guid patientId,string name,string buildingName,string appartmentNumber,
            string landmark,string longitude,string latitude,string address,bool isDefault)
        {
            Id = id;
            PatientId = patientId;
            Name = name;
            BuildingName = buildingName;
            AppartmentNumber = appartmentNumber;
            LandMark = landmark;
            Longitude = longitude;
            Latitude = latitude;
            Address = address;
            IsDefault = isDefault;
        }
    }
}
