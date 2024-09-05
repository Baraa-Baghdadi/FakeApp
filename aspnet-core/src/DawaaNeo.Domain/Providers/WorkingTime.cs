using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace DawaaNeo.Providers
{
    public class WorkingTime : Entity<Guid>
    {
        public virtual Guid ProviderId { get; set; }
        
        public virtual ProviderWorkDay WorkDay { get; set; }

        [NotNull]
        public virtual string From { get; set; }

        [NotNull]
        public virtual string To { get; set; }

        public void SetId(Guid id) { Id = id; }

        protected WorkingTime() { }

        public WorkingTime(Guid id,Guid providerId,ProviderWorkDay workDay,string from,string to)
        {
            Id = id;
            ProviderId = providerId; 
            WorkDay = workDay;
            From = from;    
            To = to;
        }

    }
}
