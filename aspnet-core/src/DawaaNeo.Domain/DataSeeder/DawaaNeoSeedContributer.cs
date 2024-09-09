using DawaaNeo.Notifications;
using DawaaNeo.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace DawaaNeo.DataSeeder
{
    public class DawaaNeoSeedContributer : ITransientDependency
    {
        private readonly IRepository<Notification, Guid> _notificationRepo;
        private readonly IGuidGenerator _guidGenerator;

        public DawaaNeoSeedContributer(IRepository<Notification, Guid> notificationRepo, IGuidGenerator guidGenerator)
        {
            _notificationRepo = notificationRepo;
            _guidGenerator = guidGenerator;
        }

        public async Task Seed()
        {
            await CreateNotification();
        }


        private async Task CreateNotification()
        {
            Notification notification = new Notification(_guidGenerator.Create(),_guidGenerator.Create(),false,"test","test",
                NotificationTypeEnum.NewPatient,null,new Guid("3f908324-611a-2fcc-576a-3a14e528930d"),null,0);

            if (!await _notificationRepo.AnyAsync())
            {
                await _notificationRepo.InsertAsync(notification, true);
            }

        }
    }
}
