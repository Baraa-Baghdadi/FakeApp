using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace DawaaNeo.SharedDomains
{
    public class Country : Entity<int>
    {
        public string Name { get; private set; }
        public string ISOCode2 { get; private set; }
        public string ISOCode3 { get; private set; }
        public Guid CurrencyId { get; private set; }
        public Currency Currency { get; set; }

        public virtual ICollection<City> Cities { get; private set; }


        private Country(string name, string iSOCode2, string iSOCode3, Guid currencyId)
        {
            Name = name;
            ISOCode2 = iSOCode2;
            ISOCode3 = iSOCode3;
            CurrencyId = currencyId;
            Cities = new List<City>();
        }

        public static Country Create(string name, string iSOCode2, string iSOCode3, Guid currencyId)
        {
            return new Country(name, iSOCode2, iSOCode3, currencyId);
        }

        public void AddCity(City state)
        {
            Cities.Add(state);
        }
    }
}
