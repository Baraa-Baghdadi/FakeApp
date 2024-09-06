using DawaaNeo.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace DawaaNeo.Notifications
{
    public class EfCoreNotificationRepository : EfCoreRepository<DawaaNeoDbContext, Notification, Guid>, INotificationRepository
    {
        public EfCoreNotificationRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<(IEnumerable<Notification>, int)> GetUserNotifications(Guid userId, int skipCount, int maxResultCount, string? sorting)
        {
            var notification = (await GetQueryableAsync())
                .WhereIf(true, n => n.UserId == userId)
                .OrderBy(string.IsNullOrEmpty(sorting) ? NotificationConst.GetDefaultSorting(false) : sorting)
                .AsEnumerable();

            var count = notification.Count();
            notification = notification.Skip(skipCount).Take(maxResultCount);
            return (notification, count);
        }
        public async Task<(IEnumerable<Notification>, int)> GetProviderNotifications(Guid tenantId, int skipCount, int maxResultCount, string? sorting)
        {
            var notification = (await GetQueryableAsync())
                .WhereIf(true, n => n.TenantId == tenantId)
                .OrderBy(string.IsNullOrEmpty(sorting) ? NotificationConst.GetDefaultSorting(false) : sorting)
                .AsEnumerable();

            var count = notification.Count();
            notification = notification.Skip(skipCount).Take(maxResultCount);
            return (notification, count);
        }

    }
}
