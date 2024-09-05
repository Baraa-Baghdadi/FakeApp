using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DawaaNeo.Hub
{
    public interface IHubClient
    {
        Task SendMessage(string msg);
    }
}
