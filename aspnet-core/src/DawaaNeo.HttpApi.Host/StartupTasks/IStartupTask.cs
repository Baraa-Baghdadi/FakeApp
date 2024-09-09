using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DawaaNeo.StartupTasks
{
    public interface IStartupTask
    {
        Task Excute(IHost host);
    }
}
