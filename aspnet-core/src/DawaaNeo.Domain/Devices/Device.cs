using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Devices
{
    public class Device : FullAuditedAggregateRoot<Guid>
    {
        public string DeviceToken { get; set; } = string.Empty;
        public Guid UserId { get; set; }

        public Device(Guid id,Guid userId,string deviceToken)
        {
            Id = id;
            UserId = userId;
            DeviceToken = deviceToken;
        }

        public Device()
        {
            
        }
    }
}
