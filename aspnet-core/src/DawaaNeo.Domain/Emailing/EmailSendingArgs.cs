using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Emailing
{
    public class EmailSendingArgs
    {
        public string? Template { get; set; }
        public string? TargetEmail { get; set; }
        public string? ConfirmationLink { get; set; }
        public string? EmailPlaceHolder { get; set; }
        public string? PharmacyNamePlaceHolder { get; set; }
        public string? TenantPlaceHolder { get; set; }
        public Guid ProviderId { get; set; }
    }
}
