using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DawaaNeo.Hub
{
    public interface IHubClient : IApplicationService
    {
        Task PatientAddedYouMsg(string msg);
    }
}
