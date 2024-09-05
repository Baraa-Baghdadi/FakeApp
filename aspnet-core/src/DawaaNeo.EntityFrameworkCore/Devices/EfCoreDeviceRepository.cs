using DawaaNeo.EntityFrameworkCore;
using DawaaNeo.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
namespace DawaaNeo.Devices
{
    public class EfCoreDeviceRepository : EfCoreRepository<DawaaNeoDbContext, Device, Guid>, IDeviceRepository
    {
        public EfCoreDeviceRepository(IDbContextProvider<DawaaNeoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<bool> AddDevice(Guid userId, string userToken)
        {
            try
            {
                var device = await FindAsync(d => d.UserId == userId && d.DeviceToken == userToken)
                    ?? await InsertAsync(new Device { UserId = userId, DeviceToken = userToken });
                return true;
            }
            catch { 
                return false;
            }
        }
    }
}
