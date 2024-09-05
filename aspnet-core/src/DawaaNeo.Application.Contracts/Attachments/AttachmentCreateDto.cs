using System;
using System.Collections.Generic;
using System.Text;

namespace DawaaNeo.Attachments
{
  public class AttachmentCreateDto
  {
    public string Blop { get; set; }
    public string FileType { get; set; }
    public string FileName { get; set; }
    public int FileSize { get; set; }
    public AttachmentCreateDto()
    {
        
    }

    public AttachmentCreateDto(string blop, string fileType, string fileName, int fileSize)
    {
      Blop = blop;
      FileType = fileType;
      FileName = fileName;
      FileSize = fileSize;
    }
  }
}
