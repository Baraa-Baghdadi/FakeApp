using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace DawaaNeo.Providers
{
    public class ProviderManager : DomainService
    {
        protected IProviderRepository _providerRepository;
        public ProviderManager(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public virtual async Task<Provider> CreateAsync(string email,string pharmacyName,string pharmacyPhone,
            double latitude,double longitude,string address,int cityId,List<WorkingTime> workingTimes,Guid tenantId)
        {
            workingTimes.ForEach(x =>  x.SetId(GuidGenerator.Create()));

            var provider = new Provider(GuidGenerator.Create(),email,pharmacyName,pharmacyPhone, latitude, longitude,
                address,cityId,workingTimes,tenantId);

            return await _providerRepository.InsertAsync(provider,true);
        }

        public virtual async Task<Provider> UpdateAsync(Guid id,string? currencyStamp=null)
        {
            var provider = await _providerRepository.GetAsync(id);

            provider.SetConcurrencyStampIfNotNull(currencyStamp);

            return await _providerRepository.UpdateAsync(provider,true);
        }
}
}
