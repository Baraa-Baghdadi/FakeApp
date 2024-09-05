using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Devices
{
    public interface IDeviceRepository : IRepository<Device,Guid>
    {
        Task<bool> AddDevice(Guid userId, string userToken);
    }

}
