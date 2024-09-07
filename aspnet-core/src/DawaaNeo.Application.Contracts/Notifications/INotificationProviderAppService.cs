using DawaaNeo.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Notifications
{
    public interface INotificationProviderAppService : IApplicationService
    {
        Task CreateAddedYouToMyPharmacytNotification(Guid id, string content, NotificationTypeEnum type, Dictionary<string, string> extraproperties);
        Task<PagedResultDto<ProviderNotificationDto>> GetListOfProviderNotification(PagedAndSortedResultRequestDto input);
        Task MarkAllAsRead();
        Task<int> GetCountOfUnreadingMsgAsync();

    }
}
