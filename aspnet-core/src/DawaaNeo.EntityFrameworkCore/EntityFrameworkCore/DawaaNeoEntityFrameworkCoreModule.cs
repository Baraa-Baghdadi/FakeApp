using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using DawaaNeo.Devices;
using DawaaNeo.Notifications;
using Volo.Abp.BlobStoring.FileSystem;
using DawaaNeo.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;

namespace DawaaNeo.EntityFrameworkCore;

[DependsOn(
    typeof(DawaaNeoDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule)
    )]
[DependsOn(typeof(BlobStoringDatabaseEntityFrameworkCoreModule))]
    public class DawaaNeoEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        DawaaNeoEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DawaaNeoDbContext>(options =>
        {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
            options.AddRepository<Provider, Providers.EfCoreProviderRepository>();
            options.AddRepository<Patient, Patients.EfCorePatientRepository>();
            options.AddRepository<PatientProvider, Patients.EfCorePatientProviderRepository>();
            options.AddRepository<PatientAddress, Patients.EfCorePatientAddressRepository>();
            options.AddRepository<Device, Devices.EfCoreDeviceRepository>();
            options.AddRepository<Notification, Notifications.EfCoreNotificationRepository>();
            options.AddRepository<DawaaNeo.Services.Service, EfCoreServiceRepository>();
        });

        Configure<AbpDbContextOptions>(options =>
        {
                /* The main point to change your DBMS.
                 * See also DawaaNeoMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlServer();
        });

        // For Blob:
        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureDefault(container =>
            {
                container.IsMultiTenant = false;
                container.UseDatabase();
            });
        });

    }
}
