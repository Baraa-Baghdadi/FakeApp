using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Notifications
{
    public class ProviderNotificationDto : FullAuditedEntityDto<Guid>
    {
        public Guid? EntityId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public bool IsRead { get; set; }
        public decimal CreatedOn { get; set; } // time according user
    }
}
