using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Providers
{
    public class WorkingTimeDto : EntityDto<Guid>
    {
        public Guid ProviderId { get; set; }
        public ProviderWorkDay WorkDay { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
    }
}
