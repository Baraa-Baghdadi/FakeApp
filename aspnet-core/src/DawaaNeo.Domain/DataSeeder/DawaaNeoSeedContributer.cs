using DawaaNeo.Devices;
using DawaaNeo.Notifications;
using DawaaNeo.Orders;
using DawaaNeo.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace DawaaNeo.DataSeeder
{
    public class DawaaNeoSeedContributer : ITransientDependency
    {
        private readonly IRepository<Notification, Guid> _notificationRepo;
        private readonly IRepository<Patient, Guid> _patientRepo;
        private readonly IRepository<Device, Guid> _deviceRepo;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IdentityUserManager _userManager;

        public DawaaNeoSeedContributer(IRepository<Notification, Guid> notificationRepo, IGuidGenerator guidGenerator,
            IRepository<Patient, Guid> patientRepo, IdentityUserManager userManager, IRepository<Device, Guid> deviceRepo)
        {
            _notificationRepo = notificationRepo;
            _guidGenerator = guidGenerator;
            _patientRepo = patientRepo;
            _userManager = userManager;
            _deviceRepo = deviceRepo;
        }

        public async Task Seed()
        {
            await CreateMainPatinet();
        }



        private async Task CreateMainPatinet()
        {
            if (!await _patientRepo.AnyAsync())
            {
                var patientId = _guidGenerator.Create();
                IdentityUser newUser;
                Patient patient = new Patient(patientId, "932912812", "+963", "111111", "Mhd Baraa Al Baghdadi", DateTime.Now);
                var isUserExist = await _userManager.FindByEmailAsync($"{patient.CountryCode + patient.MobileNumber}@email");
                if( isUserExist == null ) {
                    await _patientRepo.InsertAsync(patient, true);
                    var username = $"{patient.CountryCode + patient.MobileNumber}";
                    var newEmail = $"{patient.CountryCode + patient.MobileNumber}@email";
                    var password = "P@ssw0rd";
                    newUser = new IdentityUser(_guidGenerator.Create(), username, newEmail, null);
                    newUser.SetIsActive(true);
                    newUser.SetEmailConfirmed(true);
                    await _userManager.CreateAsync(newUser, password);
                    Device device = new Device(_guidGenerator.Create(), newUser.Id, "eUoaFCjqmw9aezqGok5Fet:APA91bGjCvWexnUjT-C5nsR6FKQoKQtV7wuBxCkcI6FE3eYEgSvtGfcz2_fZ5qNE3BBt2NQ5sqJFGJfq89xPYxtRQja54N68Dpc6BQ7dffSdqUZNx-Aw-TUV55LjW3t_zVRZc-y1_BZ0");
                    await _deviceRepo.InsertAsync(device, true);
                }
            }
        }
    }
}
