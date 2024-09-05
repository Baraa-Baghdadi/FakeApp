using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Providers
{
    public class WorkingTimeForMobileDto
    {
        public ProviderWorkDay WorkDay { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
    }
}
