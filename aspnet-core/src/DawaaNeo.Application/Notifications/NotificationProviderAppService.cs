using DawaaNeo.Hub;
using DawaaNeo.Notifications.Mobile;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace DawaaNeo.Notifications
{

    public class NotificationProviderAppService : DawaaNeoAppService, INotificationProviderAppService
    {
        private readonly IRepository<PatientProvider> _patientProviderRepo;
        private readonly IRepository<Provider> _providerRepo;
        private readonly IRepository<Patient> _patientRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        private readonly BroadcastHub _hub;
        private readonly IDataFilter _dataFilter;

       
        public NotificationProviderAppService(IRepository<PatientProvider> patientProviderRepo,
            INotificationRepository notificationRepo,
            BroadcastHub hub, IRepository<Provider> providerRepo,
            IHubContext<BroadcastHub, IHubClient> hubContext, IDataFilter dataFilter,
            IRepository<Patient> patientRepo)
        {
            _patientProviderRepo = patientProviderRepo;
            _notificationRepo = notificationRepo;
            _hub = hub;
            _providerRepo = providerRepo;
            _hubContext = hubContext;
            _dataFilter = dataFilter;
            _patientRepo = patientRepo;
        }

        public async Task CreateAddedYouToMyPharmacytNotification(Guid id, string content, NotificationTypeEnum type, Dictionary<string, string> extraproperties)
        {
            // Get patientProvider:
            var patientProvider = await _patientProviderRepo.FirstOrDefaultAsync(row => row.PatientId == id)
                ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);

            var patient = await _patientRepo.FirstOrDefaultAsync(row => row.Id == id);

            using (_dataFilter.Disable<IMultiTenant>())
            {
                // Get Provider:
                var provider = await _providerRepo.FirstOrDefaultAsync(row => row.Id == patientProvider.ProviderId)
                    ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);

                // title for notification:
                var title = provider.PharmacyName;

                var notification = new Notification
                {
                    EntityId = id,
                    Title = title,
                    Content = content,
                    IconThumbnail = "",
                    IsRead = false,
                    Type = type,
                    TenantId = provider.TenantId,
                    CreatedOn = ServiceHelper.getTimeSpam(DateTime.UtcNow)!.Value
                };

                // save json in extraProperty Column in DB:
                if (!extraproperties.IsNullOrEmpty())
                {
                    foreach (var item in extraproperties.Keys)
                    {
                        notification.SetProperty(item, extraproperties[item]);
                    }
                }

                await _notificationRepo.InsertAsync(notification, true);

                var allConnections = _hub.getAllConnectionsId();

                if (allConnections.Any())
                {
                    var tenantId = provider.TenantId ?? Guid.Empty;
                    var connectionsId = allConnections[tenantId] ?? null;

                    if (connectionsId is not null)
                    {
                        foreach (var connectionId in connectionsId)
                        {
                            await sendMessage(connectionId, patient.Name);
                        }
                    }

                }
            }

        }


        public async Task<PagedResultDto<ProviderNotificationDto>> GetListOfProviderNotification(
            PagedAndSortedResultRequestDto input)
        {
            var currentTenant = CurrentUser.TenantId 
                ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);

            (var notifications,int count) = await _notificationRepo.GetProviderNotifications(currentTenant,
                input.SkipCount,input.MaxResultCount,input.Sorting);

            var mappingData = ObjectMapper.Map<IEnumerable<Notification>, 
                IEnumerable<ProviderNotificationDto>>(notifications).ToList();

            var requiredData = new PagedResultDto<ProviderNotificationDto>
            {
                TotalCount = count,
                Items = notifications.Select(x => new ProviderNotificationDto
                {
                    Id = x.Id,
                    IsRead = x.IsRead,
                    EntityId = x.EntityId,
                    Title = L[x.Title],
                    CreatedOn = GetRelativeDate(UnixTimeStampToDateTime((double)x.CreatedOn)),
                    CreationTime = x.CreationTime,
                    Type = x.Type,
                    Content = L[x.Content, x.GetProperty("patientName") ?? "" ]
                }).ToList()
            };

            return requiredData;
        }

        public async Task MarkAllAsRead()
        {
            var currentTenant = CurrentUser.TenantId
                ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);
            var notifications = await _notificationRepo.GetListAsync(row => row.TenantId == currentTenant && !row.IsRead);
            notifications.ForEach(notification => notification.IsRead = true);
            await _notificationRepo.UpdateManyAsync(notifications);
        
        }

        public async Task<int> GetCountOfUnreadingMsgAsync()
        {
            var currentTenant = CurrentUser.TenantId
                ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);
            var count = await _notificationRepo.CountAsync(row => row.TenantId == currentTenant && !row.IsRead);
            return count;
        }



        #region methods
        private async Task sendMessage(string connectionId,string msg)
        {
            await _hubContext.Clients.Client(connectionId).PatientAddedYouMsg(msg);
        }

        private string GetRelativeDate(DateTime date)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            switch (delta)
            {
                case < 60:
                    return L["justNow"];
                case < 120:
                    return L["oneMinuteAgo"];
                case < 3600:
                    return L["minutesAgo",ts.Minutes];
                case < 7200:
                    return L["oneHourAgo"];
                case < 86400:
                    return L["hoursAgo",ts.Hours];
                default:
                    if (date.Date == DateTime.Now.Date)
                    {
                        return L["today"];
                    }
                    else if (date.Date == DateTime.Now.AddDays(-1).Date)
                    {
                        return L["yesterday"];
                    }
                    else
                    {
                        return date.ToString("yyyy/MM/dd");
                    }
            }
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        #endregion



    }
}
