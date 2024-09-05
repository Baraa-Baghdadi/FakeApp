
using DawaaNeo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Values;

namespace DawaaNeo.Orders
{
  public class DeliveryAddress : ValueObject
  {
    public virtual string? Name { get; set; }
    public virtual string? BuildingName { get; set; }
    public virtual string? AppartmentNumber { get; set; }
    public virtual string? LandMark { get; set; }
    public virtual string? Longitude { get; set; }
    public virtual string? Latitude { get; set; }
    public virtual string? Address { get; set; }
    public virtual PatientAddressType? Type { get; set; }

    public DeliveryAddress(string? name, string? buildingName, string? appartmentNumber, string? landMark,
     string? longitude, string? latitude, string? address, PatientAddressType? type)
    {
      Name = name;
      BuildingName = buildingName;
      AppartmentNumber = appartmentNumber;
      LandMark = landMark;
      Longitude = longitude;
      Latitude = latitude;
      Address = address;
      Type = type;
    }


    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return Name;
      yield return BuildingName;
      yield return AppartmentNumber;
      yield return LandMark;
      yield return Longitude;
      yield return Latitude;
      yield return Address;
      yield return Type;
    }

    public DeliveryAddress()
    {
        
    }
  }
}
