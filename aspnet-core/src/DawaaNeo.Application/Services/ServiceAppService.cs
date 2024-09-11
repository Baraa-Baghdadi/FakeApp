using DawaaNeo.Attachments;
using DawaaNeo.Patients;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Services
{
    public class ServiceAppService : DawaaNeoAppService, IServiceAppService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IAttachmentAppService _attachmentAppService;
        private readonly IRepository<Attachment,Guid> _attachmentRepository;
        private readonly IBlobContainer _blobContainer;

        public ServiceAppService(IServiceRepository serviceRepository, IAttachmentAppService attachmentAppService,
            IRepository<Attachment,Guid> attachmentRepository, IBlobContainer blobContainer)
        {
            _serviceRepository = serviceRepository;
            _attachmentAppService = attachmentAppService;
            _attachmentRepository = attachmentRepository;
            _blobContainer = blobContainer;
        }

        public async Task<PagedResultDto<ServiceDto>> GetListAync(GetServieInput input)
        {
            var totalCount = await _serviceRepository.GetCountAsync(input.FilterText);
            var items = await _serviceRepository.GetListAsync(input.FilterText,
                input.Sorting, input.MaxResultCount, input.SkipCount);

            var mappingData = ObjectMapper.Map<List<Service>, List<ServiceDto>>(items);

            foreach (var item in mappingData)
            {

                var service = items.FirstOrDefault(row => row.Id == item.Id);
                var imageId = service!.ImageId;
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

            // For show image as it in FE:
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

            }

            var result = await _serviceRepository.UpdateAsync(dbSerice, true);
            return ObjectMapper.Map<Service, ServiceDto>(result);
        }

        public async Task<IRemoteStreamContent> DownloadFileAsync(string attachmentId)
        {
            try
            {
                Guid attachmentGuid = new Guid(attachmentId);
                var docObg = await _attachmentRepository.GetAsync(attachmentGuid);
                if (docObg != null)
                {
                    var stream = await _blobContainer.GetAsync(docObg.BlopName);
                    return new RemoteStreamContent(stream,docObg.FileName);
                }

                throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.AttachmentNotExist);
            }
            catch(Exception ex) {

                throw new UserFriendlyException(DawaaNeoDomainErrorCodes.GeneralErrorCode.AttachmentFailedDonload);

            }
        }

    }
}
