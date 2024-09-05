using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Patients
{
    public class GetPatientInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? MobileNumber { get; set; }
        public string? CountryCode { get; set; }

        public GetPatientInput()
        {
            
        }
    }
}
