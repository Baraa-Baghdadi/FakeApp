using DawaaNeo.ApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Notifications.Mobile
{
    public interface INotificationMobileAppService : IApplicationService
    {
        Task<Response<string>> MarkAsRead(Guid notificationId);
        Task<Response<int>> UnReadNotificationCount();
        Task<Response<PagedResultDto<NotificationDto>>> GetUserNotifications(PagedAndSortedResultRequestDto input);
        Task CreatOrderNotification(Guid orderId, string title, string content, NotificationTypeEnum type,
            Dictionary<string, string> extraproperties);

    }
}
