using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace DawaaNeo.Orders
{
  public class OrderItem : Entity<Guid>
  {
    public Guid OrderId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Instruction { get; set; }
    public bool IsOtc { get; set; }
    public Guid OrginalId { get; set; } //ProviderProduct || ProviderDrug
    public OrderItemType Type { get; set; }
    public string Name { get; set; }
    public string? CategoryName { get; set; }


    public OrderItem(Guid id,Guid orderId, decimal price, int quantity, string? instruction, bool isOtc,
      Guid orginalId, OrderItemType type, string name, string? categoryName)
    {
      Id = id;
      OrderId = orderId;
      Price = price;
      Quantity = quantity;
      Instruction = instruction;
      IsOtc = isOtc;
      OrginalId = orginalId;
      Type = type;
      Name = name;
      CategoryName = categoryName;
    }

    public OrderItem()
    {
        
    }

    public void SetId(Guid id) { Id = id; }
  }
}
