using DawaaNeo.ApiResponse;
using DawaaNeo.ApiResponses;
using DawaaNeo.Devices;
using DawaaNeo.Orders;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace DawaaNeo.Notifications
{
    [RemoteService (IsEnabled = false)]
    public class NotificationMobileAppService : DawaaNeoAppService, INotificationMobileAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly INotificationRepository _notificationRepo;
        private readonly IDeviceRepository _deviceRepo;
        private readonly IRepository<Order, Guid> _orderRepo;
        private readonly DataFilter _datafilter;
        private readonly ICurrentPatient _currentPatient;
        private readonly IProviderRepository _providerRepo;
        private readonly IPatientRepository _patientRepo;
        private readonly IApiResponse _apiResponse;
        private readonly ICurrentUser _currentUser;

        public NotificationMobileAppService(IdentityUserManager userManager, 
            INotificationRepository notificationRepo, IDeviceRepository deviceRepo, DataFilter datafilter,
            IRepository<Order, Guid> orderRepo,
            ICurrentPatient currentPatient, IProviderRepository providerRepo, IPatientRepository patientRepo, 
            IApiResponse apiResponse, ICurrentUser currentUser)
        {
            _userManager = userManager;
            _notificationRepo = notificationRepo;
            _deviceRepo = deviceRepo;
            _datafilter = datafilter;
            _orderRepo = orderRepo;
            _currentPatient = currentPatient;
            _providerRepo = providerRepo;
            _patientRepo = patientRepo;
            _apiResponse = apiResponse;
            _currentUser = currentUser;
        }

        public async Task CreatOrderNotification(Guid orderId, string title, string content, NotificationTypeEnum type, Dictionary<string, string> extraproperties)
        {
            Provider? provider;
            Volo.Abp.Identity.IdentityUser? user;
            var order = await _orderRepo.GetAsync(row => row.Id == orderId)
                ?? throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);

            var patient = await _patientRepo.GetAsync(row => row.Id == order.PatientId)
                ?? throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);

            using (_datafilter.Disable<IMultiTenant>())
            {
                user = await _userManager.FindByNameAsync(patient.FullMobileNumber!);

                // get order creator:

                provider = await _providerRepo.FirstOrDefaultAsync(x => x.TenantId == _currentUser.TenantId && x.Id == order.ProviderId)
                    ?? throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);
            }

            var notification = new Notification {
                EntityId = orderId,
                Title = title,
                Content = content,
                IconThumbnail = "",
                IsRead = false,
                Type = type,
                UserId = user!.Id,
                CreatedOn = ServiceHelper.getTimeSpam(DateTime.UtcNow)
            };

            // save json in extraProperty Column in DB:
            if (!extraproperties.IsNullOrEmpty())
            {
                foreach (var item in extraproperties.Keys)
                {
                    notification.SetProperty(item, extraproperties[item]);
                }
            }

            await _notificationRepo.InsertAsync(notification,true);

            // Send Notification Now:

            try
            {
                var messagBody = _getMessageBody(content, order.OrderId, provider.PharmacyName,patient.CurrentLanguage);
                await SendNotification(user.Id, title, messagBody, 
                    new Dictionary<string, string> { { "notificationId", notification.Id.ToString() }, { "entityId", order.Id.ToString() }, { "orderId",order.OrderId } });
            }

            catch { }
        }

        public async Task<Response<PagedResultDto<NotificationDto>>> GetUserNotifications(PagedAndSortedResultRequestDto input)
        {
            var currentUserId = CurrentUser.Id ?? 
                throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);
            (var notification, int count) = await _notificationRepo.GetUserNotifications(currentUserId, input.SkipCount,
                input.MaxResultCount, input.Sorting);
            var patient = await _patientRepo.FirstOrDefaultAsync(row => row.FullMobileNumber == CurrentUser.UserName);

            return _apiResponse.Success(new PagedResultDto<NotificationDto>
            {
                TotalCount = count,
                Items = notification.Select(x => new NotificationDto
                {
                    Id = x.Id,
                    IsRead = x.IsRead,
                    EntityId = x.EntityId,
                    Title = L[x.Title],
                    IconThumbnail = x.IconThumbnail,
                    CreatedOn = x.CreatedOn,
                    Type = x.Type,
                    Content = L[x.Content, x.GetProperty("orderId") ?? "", x.GetProperty("pharmacy") ?? ""]
                }).ToList()
            });
        }

        public async Task<Response<string>> MarkAsRead(Guid notificationId)
        {
            var notification = await _notificationRepo.GetAsync(notificationId)
                ?? throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);
            notification.IsRead = true;
            await _notificationRepo.UpdateAsync(notification);
            return _apiResponse.Success(L[DawaaNeoConsts.Success].ToString());
        }

        public async Task<Response<int>> UnReadNotificationCount()
        {
            var currentUser = CurrentUser.Id ?? 
                throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);
            var count = await _notificationRepo.CountAsync(n => n.UserId == currentUser && !n.IsRead);
            return _apiResponse.Success(count);
            
        }


        #region Methods

        string _getMessageBody(string content,string entityId , string pharmacyName, ApplicationLanguage culture)
        {
            var orginalCulture = CultureInfo.CurrentCulture;
            if (culture == ApplicationLanguage.ar)
            {
                CultureInfo.CurrentCulture = new CultureInfo("ar");
            }
            else CultureInfo.CurrentCulture = new CultureInfo("en");

            string message = L.GetString(content, [entityId, pharmacyName]);
            CultureInfo.CurrentCulture = orginalCulture;
            return message;

        }

        // Send Notification:
        private async Task SendNotification(Guid userId,string title,string content, Dictionary<string, string> extraProperties)
        {
            var devices = await _deviceRepo.GetListAsync(d => d.UserId == userId);

            foreach (var device in devices)
            {
                try
                {
                    var messaging = FirebaseMessaging.DefaultInstance;
                    var result = await messaging.SendAsync(new Message()
                    {
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = title,
                            Body = content
                        } ,
                        Data = extraProperties,
                        Token = device.DeviceToken
                    });
                    if (!string.IsNullOrEmpty(result))
                    {
                        // Message Send Succefuly:
                        Logger.LogDebug("Message Sent Succefuly");
                    }
                    else
                    {
                        // Message Not Send Succefuly:
                        Logger.LogDebug("Message Not Sent Succefuly");
                    }

                }
                // when user uninstall application without logOut:
                catch(FirebaseMessagingException ex)
                {
                    Logger.LogCritical(ex.Message + " " + (ex.InnerException is null ? "" : ex.InnerException.Message));
                    await _deviceRepo.DeleteAsync(device, true);
                }
                catch(Exception e)
                {
                    Logger.LogCritical(e.Message + " " + (e.InnerException is null ? "" : e.InnerException.Message));

                }

            }
        }

        #endregion


    }
}
