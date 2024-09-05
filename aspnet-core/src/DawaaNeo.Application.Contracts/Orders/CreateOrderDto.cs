using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Orders
{
  public class CreateOrderDto
  {
    public Guid PatientId { get; set; }
    public bool HasInsurance { get; set; }
    public string Currency { get; set; }
    public int? Decimals { get; set; }
    public OrderType OrderType { get; set; }
    public PaymentMethodTypes PaymentMethodTypes { get; set; }
    public DateTime DeliveryDate { get; set; }
    public decimal? DeliveryCost { get; set; }
    public Guid AddressId { get; set; }
    public string? Note { get; set; }
    public string? Instruction { get; set; }
    public List<OrderItemDto> Items { get; set; }

  }

}
