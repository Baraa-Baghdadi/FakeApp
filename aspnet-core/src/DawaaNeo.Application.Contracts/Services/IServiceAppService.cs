using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace DawaaNeo.Services
{
    public interface IServiceAppService
    {
        Task<ServiceDto> CreateServiceAsync(CreateServiceDto input);
        Task<PagedResultDto<ServiceDto>> GetListAync(GetServieInput input);
        Task<ServiceDto> UpdateAsync(Guid id, CreateServiceDto input);
        Task<ServiceDto> GetServiceAsync(Guid id);
        Task<IRemoteStreamContent> DownloadFileAsync(string attachmentId);
    }
}