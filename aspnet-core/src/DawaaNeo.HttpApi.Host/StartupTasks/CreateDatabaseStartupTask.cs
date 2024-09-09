using Castle.Core.Configuration;
using DawaaNeo.DataSeeder;
using DawaaNeo.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DawaaNeo.StartupTasks
{
    public class CreateDatabaseStartupTask : IStartupTask
    {
        private readonly SeederService _seederService;

        public CreateDatabaseStartupTask(SeederService seederService)
        {
            _seederService = seederService;
        }

        public async Task Excute(IHost host)
        {
            try
            {
                await _seederService.SeedAsync();
            }
            catch
            {
                
            }
        }
    }
}
