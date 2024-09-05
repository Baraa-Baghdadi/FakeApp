using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Notifications
{
    public class Notification : FullAuditedAggregateRoot<Guid>
    {
        public Guid? EntityId { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public Guid UserId { get; set; }
        public string? IconThumbnail { get; set; } = string.Empty;
        public decimal CreatedOn { get; set; } // time according user
    }
}
