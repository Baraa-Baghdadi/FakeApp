using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Orders
{
  public class OrderItemDto
  {
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? Instruction { get; set; }
    public bool IsOtc { get; set; }
    public Guid OrginalId { get; set; } //ProviderProduct || ProviderDrug
    public OrderItemType Type { get; set; }
    public string Name { get; set; }
    public string? CategoryName { get; set; }

  }
}
