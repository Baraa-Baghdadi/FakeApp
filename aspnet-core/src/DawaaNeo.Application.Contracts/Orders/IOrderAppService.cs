using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Orders
{
  public interface IOrderAppService 
  {
    Task<OrderDto> CreateAsync(CreateOrderDto input);
  }
}
