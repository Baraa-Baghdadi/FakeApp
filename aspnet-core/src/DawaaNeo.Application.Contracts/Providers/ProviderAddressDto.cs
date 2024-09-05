using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Providers
{
    public class ProviderAddressDto : EntityDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = null!;
        public int CityId { get; set; }
    }
}
