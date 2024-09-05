namespace DawaaNeo.Permissions;

public static class DawaaNeoPermissions
{
    public const string GroupName = "DawaaNeo";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class Dashboard
    {
        public const string DashboardGroup = GroupName + ".Dashboard";
        public const string Host = DashboardGroup + ".Host";
        public const string Tenant = DashboardGroup + ".Tenant";

    }

    // Name of Permession:
    public static class Patients
    {
        public const string Default = GroupName + ".Patients";

    }

    // Name of Permession:
    public static class Providers
    {
        public const string Default = GroupName + ".Providers";
        public const string downloadQrCode = GroupName + ".Providers.downloadQrCode";

    }
}
