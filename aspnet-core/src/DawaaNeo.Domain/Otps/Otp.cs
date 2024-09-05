using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Otps
{
    public class Otp : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public string MobileNumber { get; set; }
        public bool IsUsed { get; set; }
        [NotNull]
        public string Code { get; set; }

        protected Otp() { }

        public Otp(Guid id,string mobileNumber, string code)
        {
            // note: put validation here if you want
            Id = id; MobileNumber = mobileNumber; IsUsed = false; Code = code;
        }
    }
}
