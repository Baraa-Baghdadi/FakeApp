using DawaaNeo.SharedDomains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Values;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
namespace DawaaNeo.Providers.valueObject
{
    public class ProviderAddress : ValueObject
    {

        // Google Map Info

        [NotNull]
        public double Latitude { get; private set; }
        [NotNull]
        public double Longitude { get; private set; }
        [NotNull]
        public string Address { get; private set; }
        [NotNull]
        public int CityId { get; private set; }
        public virtual City City { get; set; }

        public ProviderAddress()
        {
            
        }

        private ProviderAddress(double latitude, double longitude, string address,int cityId)
        {
            //Check.NotNull(address,nameof(address));
            //Check.Length(address,nameof(address),ProviderConst.AddressMaxLength,ProviderConst.AddressMinLength);
            //Check.NotNull(latitude, nameof(latitude));
            //Check.NotNull(longitude, nameof(longitude));
            //Check.NotNull(cityId, nameof(cityId));
            Latitude = latitude;
            Longitude = longitude;
            Address = address!;
            CityId = cityId;
        }

        public static ProviderAddress Create(double latitude, double longitude, string address, int cityId)
        {
            return new ProviderAddress(latitude, longitude, address, cityId);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Latitude;
            yield return Longitude; 
            yield return Address;
            yield return CityId;
        }
    }
}
