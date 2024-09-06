using DawaaNeo.Hub;
using DawaaNeo.Notifications.Mobile;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
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
        private readonly INotificationRepository _notificationRepo;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        private readonly BroadcastHub _hub;
        private readonly IDataFilter _dataFilter;

        public NotificationProviderAppService(IRepository<PatientProvider> patientProviderRepo,
            INotificationRepository notificationRepo,
            BroadcastHub hub, IRepository<Provider> providerRepo,
            IHubContext<BroadcastHub, IHubClient> hubContext, IDataFilter dataFilter)
        {
            _patientProviderRepo = patientProviderRepo;
            _notificationRepo = notificationRepo;
            _hub = hub;
            _providerRepo = providerRepo;
            _hubContext = hubContext;
            _dataFilter = dataFilter;
        }

        public async Task CreateAddedYouToMyPharmacytNotification(Guid id, string content, NotificationTypeEnum type)
        {
            // Get patientProvider:
            var patientProvider = await _patientProviderRepo.FirstOrDefaultAsync(row => row.Id == id)
                ?? throw new UserFriendlyException(L[DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound]);

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
                    CreatedOn = ServiceHelper.getTimeSpam(DateTime.UtcNow)
                };

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
                            await sendMessage(connectionId, content);
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
                Items = mappingData
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

        #endregion

    }
}
