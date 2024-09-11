using DawaaNeo.Notifications;
using DawaaNeo.Notifications.Mobile;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace DawaaNeo.Orders
{
    // we use DawaaNeoAppService for localiztion in this service:
    public class OrderAppService : DawaaNeoAppService, IOrderAppService
  {
    private readonly IRepository<Order,Guid> _orderRepository;
    private readonly IProviderRepository _providerRepo;
    private readonly IRepository<PatientAddress,Guid> _addressRepo;
    private readonly INotificationMobileAppService _notificationAppService;
    public OrderAppService(IRepository<Order, Guid> orderRepository, IProviderRepository providerRepo,
      IRepository<PatientAddress, Guid> addressRepo, INotificationMobileAppService notificationAppService)
    {
      _orderRepository = orderRepository;
      _providerRepo = providerRepo;
      _addressRepo = addressRepo;
      _notificationAppService = notificationAppService;
    }
    public async Task<OrderDto> CreateAsync(CreateOrderDto input)
    {
      // Create Custom orderId:
      string orderId = _generateCustomOrderId(); 
      // Get provider from current user :
      Provider? provider;
      var providerTenant = CurrentUser.TenantId;
      using (DataFilter.Disable<IMultiTenant>())
      {
        provider = await _providerRepo.FirstOrDefaultAsync(x => x.TenantId == providerTenant);
      }

      // Calculate Total Price:
      var Total = input.Items!.Sum(x => x.Quantity * x.Price);

      // Convert From OrderItemDto to OrderItem:
      var items = ObjectMapper.Map<List<OrderItemDto>, List<OrderItem>>(input.Items);
      items.ForEach(x => x.SetId(GuidGenerator.Create()));

      // DeliveryCost:
      decimal? deliveryCost;
      deliveryCost = input.DeliveryCost != null ? input.DeliveryCost : ServiceHelper._calculateDeliveryCost(input.AddressId,Total);

      // Create New Order:
      var order = new Order(GuidGenerator.Create(),provider!.Id, input.PatientId, orderId, input.HasInsurance, Total, input.Currency,
        input.Decimals, input.OrderType, input.PaymentMethodTypes, ServiceHelper.getTimeSpam(DateTime.UtcNow).Value, input.DeliveryDate,
        deliveryCost, input.AddressId, input.Note, input.Instruction, items);

      if (input.OrderType == OrderType.Delivery)
      {
        // get patient address:
        var address = await _addressRepo.FirstOrDefaultAsync(x => x.Id == input.AddressId)
          ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);

        // add address details in special columns:
        order.SetDeliveryAddress(address.Name, address.BuildingName, address.AppartmentNumber, address.LandMark, address.Longitude, address.Latitude, address.Address, address.Type);

      }

      // save in DB:
      await _orderRepository.InsertAsync(order, true);

     // update quantity after create new order:
     //await ServiceHelper.UpdateQuantity(input.Items, "-");

     // Send Notification:
        await _notificationAppService.CreatOrderNotification(order.Id,provider.PharmacyName,OrderConst.OrderCreationContent,NotificationTypeEnum.Order
            ,new Dictionary<string, string> { { "orderId", order.OrderId }, { "pharmacy", provider.PharmacyName } });

      return ObjectMapper.Map<Order,OrderDto>(order);

    }


        #region Methods

        public static string _generateCustomOrderId()
        {
            const string AllowedChars = "0123456789";
            const int CodeLength = 6;
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomByte = new byte[CodeLength];
                rng.GetBytes(randomByte);
                char[] chars = new char[CodeLength];
                for (int i = 0; i < CodeLength; i++)
                {
                    int index = randomByte[i] % AllowedChars.Length;
                    chars[i] = AllowedChars[index];
                }
                return new string(chars);
            }
        }

        #endregion


    }
}
