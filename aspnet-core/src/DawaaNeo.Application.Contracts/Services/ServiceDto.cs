using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Services
{
    public class ServiceDto : EntityDto<Guid>
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }

        public string? ArTitle { get; set; }
        public Guid? ImageId { get; set; }

        public string? Icon { get; set; } // as thumbnail

        public bool IsActive { get; set; }
        public string? OrginalImage { get; set; } // return Orginal image for showing it as base 64

    }
}
