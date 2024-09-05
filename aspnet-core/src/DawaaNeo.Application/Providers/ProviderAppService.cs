using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Providers
{
    public class ProviderAppService : ApplicationService, IProviderAppService
    {
        protected IProviderRepository _providerRepository;
        protected ProviderManager _providerManager;

        public ProviderAppService(IProviderRepository providerRepository, ProviderManager providerManager)
        {
            _providerManager = providerManager;
            _providerRepository = providerRepository;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _providerRepository.DeleteAsync(id);
        }

        public virtual async Task<ProviderDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Provider,ProviderDto>(await  _providerRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<ProviderDto>> GetListAsync(GetProviderInput input)
        {
            var totaCount = await _providerRepository.GetCountAsync(input.FilterText,input.Email,input.PharmacyName, input.PharmacyPhone);
            var items = await _providerRepository.GetListAsync(input.FilterText, input.Email , input.PharmacyName, 
                input.PharmacyPhone,input.Sorting,input.MaxResultCount,input.SkipCount);
            return new PagedResultDto<ProviderDto>
            {
                TotalCount = totaCount,
                Items = ObjectMapper.Map<List<Provider>,List<ProviderDto>>(items)
            };
        }

        public virtual async Task<ProviderDto> UpdateAsync(Guid id, ProviderUpdateDto input)
        {
            var provider = await _providerManager.UpdateAsync(id, input.ConcurrencyStamp);
            return ObjectMapper.Map<Provider, ProviderDto>(provider);
        }
    }
}
