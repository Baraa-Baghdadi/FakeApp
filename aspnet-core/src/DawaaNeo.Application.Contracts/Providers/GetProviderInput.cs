using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Providers
{
    public class GetProviderInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public string? Email { get; set; }
        public string? PharmacyName { get; set; }
        public string? PharmacyPhone { get; set; }
        public GetProviderInput()
        {
            
        }
    }
}
