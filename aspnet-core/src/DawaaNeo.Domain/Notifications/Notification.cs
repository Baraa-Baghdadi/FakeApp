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
        public Guid? UserId { get; set; } // For mobile notification
        public Guid? TenantId { get; set; } // For provider notification
        public string? IconThumbnail { get; set; } = string.Empty;
        public decimal CreatedOn { get; set; } // time according user



        public Notification(Guid id,Guid? entityId, bool isRead, string title, string content, NotificationTypeEnum type, Guid? userId, Guid? tenantId, string? iconThumbnail, decimal createdOn)
        {
            Id = id;
            EntityId = entityId;
            IsRead = isRead;
            Title = title;
            Content = content;
            Type = type;
            UserId = userId;
            TenantId = tenantId;
            IconThumbnail = iconThumbnail;
            CreatedOn = createdOn;
        }

        public Notification()
        {

        }

    }



    
}
