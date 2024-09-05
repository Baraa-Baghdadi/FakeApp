using DawaaNeo.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Providers.Mobile
{
    public interface IProviderMobileAppService : IApplicationService
    {
        Task<Response<ProviderDto>> ScanQR(string providerCode);
    }
}
