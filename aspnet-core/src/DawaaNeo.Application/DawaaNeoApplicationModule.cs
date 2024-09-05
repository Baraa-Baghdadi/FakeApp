using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace DawaaNeo;

[DependsOn(
    typeof(DawaaNeoDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(DawaaNeoApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpLocalizationModule),
    typeof(AbpBlobStoringModule)
    )]
public class DawaaNeoApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.Configurators.Add(AbpAutoMapperConfigurationContext =>
            {
                var Dawaa24ApplicationAutoMapperProfile = AbpAutoMapperConfigurationContext.ServiceProvider
                .GetRequiredService<DawaaNeoApplicationAutoMapperProfile> ();

                AbpAutoMapperConfigurationContext.MapperConfiguration
                .AddProfile(Dawaa24ApplicationAutoMapperProfile);
            });
        });


        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = true;
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
