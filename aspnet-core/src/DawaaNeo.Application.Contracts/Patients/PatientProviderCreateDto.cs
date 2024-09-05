using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Patients
{
    public class PatientProviderCreateDto
    {
        public Guid ProviderId { get; set; }
        public PatientAddingType AddingType { get; set; } = ((PatientAddingType[])Enum.GetValues(typeof(PatientAddingType)))[0];
    }
}
