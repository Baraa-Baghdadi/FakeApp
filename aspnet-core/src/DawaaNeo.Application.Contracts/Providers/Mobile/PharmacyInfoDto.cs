using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Providers.Mobile
{
    public class PharmacyInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public double Longitude { get; set; }
        public double Latitude { get; set; } 
        public string Address { get; set; } = null!;
        public DateTime AddingDate { get; set; }
        public IEnumerable<WorkingTimeForMobileDto>? WorkingTimes { get; set; }
    }
}
