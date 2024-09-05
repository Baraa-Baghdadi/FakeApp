using DawaaNeo.Providers.valueObject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;


namespace DawaaNeo.Providers
{
    public class Provider : FullAuditedAggregateRoot<Guid>,IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public virtual string Email { get; set; }
        [NotNull]
        public virtual string PharmacyName { get; set; }
        [NotNull]
        public virtual string PharmacyPhone { get; set; }

        [NotNull]
        public ProviderAddress LocationInfo { get; private set; }
        public ICollection<WorkingTime> WorkingTimes { get; private set; }

        protected Provider() { }

        public Provider(Guid id,string email,string pharmacyName,string pharmacyPhone,double latitude, double longitude , 
            string address, int cityId, ICollection<WorkingTime> workingTimes,Guid? tenantId)
        {
            Id = id;
            Email = email;
            PharmacyName = pharmacyName;
            PharmacyPhone = pharmacyPhone;
            WorkingTimes = workingTimes;
            TenantId = tenantId;
            LocationInfo = ProviderAddress.Create(latitude, longitude,address,cityId);
        }
    }
}
