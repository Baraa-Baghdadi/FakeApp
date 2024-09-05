using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Providers
{
    public interface IProviderAppService : IApplicationService
    {
        Task<PagedResultDto<ProviderDto>> GetListAsync(GetProviderInput input);
        Task<ProviderDto> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<ProviderDto> UpdateAsync(Guid id,ProviderUpdateDto input);
    }
}
