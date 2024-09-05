using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Attachments
{
  public class Attachment : FullAuditedAggregateRoot<Guid>
  {
    [NotNull]
    public string BlopName { get; set; }
    [NotNull]
    public string FileType { get; set; }
    [NotNull]
    public string FileName { get; set; }
    [NotNull]
    public int FileSize { get; set; }
    protected Attachment()
    {
        
    }

    private Attachment(string blopName, string fileType, string fileName, int fileSize)
    { 
      BlopName = blopName;
      FileType = fileType;
      FileName = fileName;
      FileSize = fileSize;
    }

    public static Attachment Create (string blop, string fileType, string fileName, int fileSize)
      => new Attachment(blop, fileType, fileName, fileSize); 
  }
}
