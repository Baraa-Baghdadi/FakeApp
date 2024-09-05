using Volo.Abp.Domain.Entities;

namespace DawaaNeo.SharedDomains
{
    public class City : Entity<int>
    {
        public string Name { get; set; }


        private City(string name) {
            Name = name;
        }

        public static City Create(string name) {
            return new City(name);
        }

    }
}
