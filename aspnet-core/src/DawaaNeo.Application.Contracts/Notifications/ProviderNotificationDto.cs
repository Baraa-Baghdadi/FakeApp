using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Notifications
{
    public class ProviderNotificationDto : EntityDto<Guid>
    {
        public Guid? EntityId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? TenantId { get; set; } // For provider notification
        public decimal CreatedOn { get; set; } // time according user
    }
}
