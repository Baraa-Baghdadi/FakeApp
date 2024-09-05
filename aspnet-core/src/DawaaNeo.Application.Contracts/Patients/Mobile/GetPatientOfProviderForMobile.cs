using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Patients.Mobile
{
    public class GetPatientOfProviderForMobile : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? PharmacyName { get; set; }
        public string? PharmacyPhone { get; set; }

        public GetPatientOfProviderForMobile()
        {
            
        }
    }
}
