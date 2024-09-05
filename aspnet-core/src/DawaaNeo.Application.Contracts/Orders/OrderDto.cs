using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Orders
{
  public class OrderDto : EntityDto<Guid>
  {
        public string OrderId { get; set; }
        public bool HasInsurance { get; set; }
        public decimal Total { get; set; }
        public string? Currency { get; set; }
        public int? Decimals { get; set; }
        public OrderType OrderType { get; set; }
        public PaymentMethodTypes PaymentMethodTypes { get; set; }
        public decimal CretedOn { get; set; }

        public DateTime DeliveryDate { get; set; }
        public decimal? DeliveryCost { get; set; }
        public Guid AddressId { get; set; }
        public string? Note { get; set; }
        public string? Instruction { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }
}
