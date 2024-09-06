using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Notifications
{
    public interface INotificationRepository : IRepository<Notification,Guid>
    {
        Task<(IEnumerable<Notification>,int)> GetUserNotifications(Guid userId,int skipCount,int maxResultCount
            ,string? sorting);
        Task<(IEnumerable<Notification>, int)> GetProviderNotifications(Guid tenantId, int skipCount, int maxResultCount
            , string? sorting);
    }
}
