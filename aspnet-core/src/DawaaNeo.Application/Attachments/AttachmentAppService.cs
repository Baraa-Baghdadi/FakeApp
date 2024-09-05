using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;

namespace DawaaNeo.Attachments
{
  public class AttachmentAppService : DawaaNeoAppService, IAttachmentAppService
  {
    private readonly IBlobContainer _blobContainer;
    private IRepository<Attachment> _attachmentRepo; 
    public AttachmentAppService(IBlobContainer blobContainer, IRepository<Attachment> attachmentRepo)
    {
        _blobContainer = blobContainer;
        _attachmentRepo = attachmentRepo;
    }
    public async Task<AttachmentDto> CreateAttachmentAsync(AttachmentCreateDto input)
    {
      // store in DB as byte[] and return as base64
      var stream = new MemoryStream(Convert.FromBase64String(input.Blop));
      var blopName = GuidGenerator.Create().ToString();
      await _blobContainer.SaveAsync(blopName, stream);
      var attachmentToCreate = Attachment.Create(blopName,input.FileType,input.FileName,input.FileSize);
      var dbRecord = await _attachmentRepo.InsertAsync(attachmentToCreate,true);
      return new AttachmentDto
      {
        FileSize = input.FileSize,
        BlopName = blopName,
        FileName = input.FileName,
        FileType = input.FileType,
        Id = dbRecord.Id
      };
    }

    public async Task DeleteImage(Guid imageId)
    {
      var oldIcon = await _attachmentRepo.GetAsync(x => x.Id == imageId);
      if (oldIcon is not null)
      {
         await _attachmentRepo.DeleteAsync(oldIcon);
      }
    }

    public async Task<byte[]> GetImage(Guid? imageId)
    {
       var docObj = await _attachmentRepo.FirstOrDefaultAsync(x => x.Id == imageId);
      if (docObj is not null)
      {
        return await _blobContainer.GetAllBytesAsync(docObj.BlopName);
      }
      return null!;
    }
  }
}
