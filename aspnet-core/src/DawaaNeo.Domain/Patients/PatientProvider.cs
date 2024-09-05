using DawaaNeo.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Patients
{
    public class PatientProvider : FullAuditedEntity<Guid>
    {
        public Guid? PatientId { get; set; }
        public Guid? ProviderId { get; set; }
        public DateTime? AddingDate { get; set; }
        public Patient? Patient { get; set; }
        public Provider? Provider { get; set; }
        public PatientAddingType? AddingType { get; set; }

        protected PatientProvider() { }

        public PatientProvider(Guid id,Guid patientId,Guid providerId,DateTime addingDate,PatientAddingType addingType)
        {
            Id = id;
            PatientId = patientId;
            ProviderId = providerId;
            AddingDate = addingDate;
            AddingType = addingType;
        }
    }
}
