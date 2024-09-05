using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Providers
{
    public class VerficationCode : FullAuditedEntity<int>
    {
        public virtual string? Code { get; set; }
        public virtual string? Email { get; set; }
        public virtual DateTime? CreationDate { get; set; }

        protected VerficationCode() { }

        public VerficationCode(int id,string code,string email,DateTime creationDate)
        {
            Id = id;
            Code = code; 
            Email = email; 
            CreationDate = creationDate;
        }

    }
}
