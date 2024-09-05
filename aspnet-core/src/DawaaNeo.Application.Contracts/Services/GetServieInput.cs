using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace DawaaNeo.Services
{
    public class GetServieInput 
        : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
    }
}
