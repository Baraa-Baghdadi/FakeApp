using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace DawaaNeo.Providers
{
    public class ProviderUpdateDto : IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public IEnumerable<WorkingTimeDto>? WorkingTimes { get; set; }
    }
}
