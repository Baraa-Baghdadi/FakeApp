using DawaaNeo.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DawaaNeo.ProviderAuth
{
    public class ProviderRegisterInfoDto
    {
        public string Password { get; set; }

        public string Email { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyPhone { get; set; }
        public ProviderAddressDto LocationInfo { get; set; } = null!;
        public List<WorkingTimeDto> WorkingTimes { get; set; } = null!;
    }
}
