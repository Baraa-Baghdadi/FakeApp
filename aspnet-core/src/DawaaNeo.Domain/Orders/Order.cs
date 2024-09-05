using DawaaNeo.Enums;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Orders
{
  public class Order : FullAuditedAggregateRoot<Guid>
  {
    public Guid ProviderId { get; set; }
    public Guid PatientId { get; set; }
    public string OrderId { get; set; }
    public bool HasInsurance { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; }
    public int? Decimals { get; set; }
    public OrderType OrderType { get; set; }
    public PaymentMethodTypes PaymentMethodTypes { get; set; }
    public decimal CretedOn { get; set; }

    public DateTime DeliveryDate { get; set; }
    public decimal? DeliveryCost { get; set; }
    public Guid AddressId { get; set; }
    public string? Note { get; set; }
    public string? Instruction { get; set; }
    public DeliveryAddress DeliveryAddress { get; set; }
    public List<OrderItem>? Items { get; set; }
    public Patient Patient { get; set; }
    public Provider Provider { get; set; }



    public Order(Guid id,Guid providerId, Guid patientId,string orderId ,bool hasInsurance,
    decimal total, string currency, int? decimals,
    OrderType orderType, PaymentMethodTypes paymentMethodTypes,
    decimal cretedOn, DateTime deliveryDate,
    decimal? deliveryCost, Guid addressId,
    string? note, string? instruction,
    List<OrderItem>? items)
    {
      Id = id;
      ProviderId = providerId;
      PatientId = patientId;
      OrderId = orderId;
      HasInsurance = hasInsurance;
      Total = total;
      Currency = currency;
      Decimals = decimals;
      OrderType = orderType;
      PaymentMethodTypes = paymentMethodTypes;
      CretedOn = cretedOn;
      DeliveryDate = deliveryDate;
      DeliveryCost = deliveryCost;
      AddressId = addressId;
      Note = note;
      Instruction = instruction;
      Items = items;
    }

    public Order()
    {
        
    }

    public void setItemsValue(List<OrderItem> items) {

      Items = items;
    }

    public void SetDeliveryAddress(string? name, string? buildingName, string? appartmentNumber, string? landMark,
     string? longitude, string? latitude, string? address, PatientAddressType type)
    {
      DeliveryAddress = new DeliveryAddress(name, buildingName, appartmentNumber, landMark, longitude, latitude, address, type);
    }

  }
}
