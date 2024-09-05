using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Attachments
{
  public interface IAttachmentAppService
  {
    Task<byte[]> GetImage(Guid? imageId);
    Task<AttachmentDto> CreateAttachmentAsync(AttachmentCreateDto input);
    Task DeleteImage(Guid imageId);
  }
}
