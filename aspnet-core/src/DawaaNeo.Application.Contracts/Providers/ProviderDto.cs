using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.SimpleStateChecking;

namespace DawaaNeo.Providers
{
    public class ProviderDto : IHasConcurrencyStamp
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string? Email { get; set; }
        public string? PharmacyName { get; set; }
        public string? PharmacyPhone { get; set; }
        public string ConcurrencyStamp { get; set; } = null!;
        public virtual List<WorkingTimeDto>? WorkingTimes { get; set; }
    }
}
