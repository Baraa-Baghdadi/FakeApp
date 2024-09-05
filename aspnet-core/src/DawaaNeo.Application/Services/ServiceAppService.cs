using DawaaNeo.Attachments;
using DawaaNeo.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Services
{
    public class ServiceAppService : DawaaNeoAppService, IServiceAppService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IAttachmentAppService _attachmentAppService;

        public ServiceAppService(IServiceRepository serviceRepository, IAttachmentAppService attachmentAppService)
        {
            _serviceRepository = serviceRepository;
            _attachmentAppService = attachmentAppService;
        }

        public async Task<PagedResultDto<ServiceDto>> GetListAync(GetServieInput input)
        {
            var totalCount = await _serviceRepository.GetCountAsync(input.FilterText);
            var items = await _serviceRepository.GetListAsync(input.FilterText,
                input.Sorting, input.MaxResultCount, input.SkipCount);

            var mappingData = ObjectMapper.Map<List<Service>, List<ServiceDto>>(items);

            foreach (var item in mappingData)
            {
                var imageId = items.FirstOrDefault(row => row.Id == item.Id).ImageId;
                item.OrginalImage = Convert.ToBase64String(await _attachmentAppService.GetImage(imageId));
            }

            return new PagedResultDto<ServiceDto>
            {
                TotalCount = totalCount,
                Items = mappingData
            };
        }

        public async Task<ServiceDto> GetServiceAsync(Guid id)
        {
            var dbSerice = await _serviceRepository.FirstOrDefaultAsync(row => row.Id == id)
                ?? throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);

            return ObjectMapper.Map<Service, ServiceDto>(dbSerice);
        }


        // Just Thumbnail without attachment returned as base64:
        public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto input)
        {
            var newImage = new AttachmentCreateDto()
            {
                Blop = input.Blop,
                FileName = input.FileName,
                FileType = input.FileType,
                FileSize = input.FileSize
            };

            // Create Image in Attachment Table For Get Orginal Image:
            var orginalImage = await _attachmentAppService.CreateAttachmentAsync(newImage);

            var service = new Service()
            {
                Title = input.Title,
                ArTitle = input.ArTitle,
                IsActive = input.IsActive,
                Icon = ServiceHelper.GetThumbNail(input.Blop),
                ImageId = orginalImage.Id
            };

            var result = await _serviceRepository.InsertAsync(service, true);
            var mappingData =  ObjectMapper.Map<Service,ServiceDto>(result);

            mappingData.OrginalImage = Convert.ToBase64String(await _attachmentAppService.GetImage(result.ImageId));

            return mappingData;
        }

        public async Task<ServiceDto> UpdateAsync(Guid id, CreateServiceDto input)
        {
            var dbSerice = await _serviceRepository.FirstOrDefaultAsync(row => row.Id == id)
                 ?? throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.NotFound);

            dbSerice.Title = input.Title;
            dbSerice.ArTitle = input.ArTitle;
            dbSerice.IsActive = input.IsActive;

            if (input.IsIconUpdated)
            {
                dbSerice.Icon = ServiceHelper.GetThumbNail(input.Blop);

                // Create Image in Attachment Table For Get Orginal Image:
                var newImage = new AttachmentCreateDto()
                {
                    Blop = input.Blop,
                    FileName = input.FileName,
                    FileType = input.FileType,
                    FileSize = input.FileSize
                };
                await _attachmentAppService.DeleteImage(dbSerice.ImageId);
                var orginalImage = await _attachmentAppService.CreateAttachmentAsync(newImage);
                dbSerice.ImageId = orginalImage.Id;
            }

            var result = await _serviceRepository.UpdateAsync(dbSerice, true);
            return ObjectMapper.Map<Service, ServiceDto>(result);
        }


    }
}
