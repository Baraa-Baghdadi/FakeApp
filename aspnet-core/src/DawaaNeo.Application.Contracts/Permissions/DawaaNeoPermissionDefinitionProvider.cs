using DawaaNeo.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace DawaaNeo.Permissions;

public class DawaaNeoPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DawaaNeoPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(DawaaNeoPermissions.MyPermission1, L("Permission:MyPermission1"));

        myGroup.AddPermission(DawaaNeoPermissions.Dashboard.Host, L("Permission:Dashboard"), Volo.Abp.MultiTenancy.MultiTenancySides.Host);
        myGroup.AddPermission(DawaaNeoPermissions.Dashboard.Tenant, L("Permission:Dashboard"), Volo.Abp.MultiTenancy.MultiTenancySides.Tenant);

        var patientPermission = myGroup.AddPermission(DawaaNeoPermissions.Patients.Default, L("Permission:Patients"));
        var providerPermission = myGroup.AddPermission(DawaaNeoPermissions.Providers.Default, L("Permission:Providers"));
        var qrDownloadPermission = myGroup.AddPermission(DawaaNeoPermissions.Providers.downloadQrCode, L("Permission:DownloadQrCode"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DawaaNeoResource>(name);
    }
}
