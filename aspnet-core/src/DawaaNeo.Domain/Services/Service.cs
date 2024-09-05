using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace DawaaNeo.Services
{
    public class Service : FullAuditedEntity<Guid>
    {

        public string Title { get; set; }

        public string ArTitle { get; set; }

        public string Icon { get; set; } // as thumbnail

        public bool IsActive { get; set; }
        public Guid ImageId { get; set; }


        public Service()
        {
            
        }


        public Service(Guid id,string title, string arTitle, string icon, bool isActive, Guid imageId)
        {
            Id = id;
            Title = title;
            ArTitle = arTitle;
            Icon = icon;
            IsActive = isActive;
            ImageId = imageId;
        }

    }
}
