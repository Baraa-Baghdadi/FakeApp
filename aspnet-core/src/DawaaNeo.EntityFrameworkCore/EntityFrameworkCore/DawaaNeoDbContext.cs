using DawaaNeo.Orders;
using DawaaNeo.Otps;
using DawaaNeo.Patients;
using DawaaNeo.Providers;
using DawaaNeo.SharedDomains;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using DawaaNeo.Attachments;
using DawaaNeo.Devices;
using DawaaNeo.Notifications;
using DawaaNeo.Services;

namespace DawaaNeo.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class DawaaNeoDbContext :
    AbpDbContext<DawaaNeoDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    // second section:
    public DbSet<PatientProvider> PatientProviders { get; set; } = null!;
    public DbSet<WorkingTime> WorkingTimes { get; set; } = null!;
    public DbSet<Provider> Providers { get; set; } = null!;
    public DbSet<Otp> Otps { get; set; }
    public DbSet<PatientAddress> PatientAddresses { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<VerficationCode> VerficationCodes { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Attachment> Attachments { get; set; }


    public DbSet<Device> Devices { get; set; }
    public DbSet<Notification> Notifications { get; set; }


    public DbSet<DawaaNeo.Services.Service> Services { get; set; }


    public DawaaNeoDbContext(DbContextOptions<DawaaNeoDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(DawaaNeoConsts.DbTablePrefix + "YourEntities", DawaaNeoConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        // second section:


        builder.Entity<PatientAddress>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "PatientAddresses", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Patient>().WithMany(x => x.PatientAddresses).HasForeignKey(x => x.PatientId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        });


        // WorkingTimes
        builder.Entity<WorkingTime>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "WorkingTimes", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Provider>().WithMany(x => x.WorkingTimes).HasForeignKey(x => x.ProviderId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Patient>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Patients", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasMany(p => p.PatientAddresses).WithOne().HasForeignKey(x => x.PatientId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        });
        builder.Entity<PatientProvider>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "PatientProviders", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Otp>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Otps", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });


        builder.Entity<Provider>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Providers", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
            // For value object:
            b.OwnsOne(x => x.LocationInfo);
            b.Property(x => x.Email).HasColumnName(nameof(Provider.Email)).IsRequired();
            b.Property(x => x.PharmacyName).HasColumnName(nameof(Provider.PharmacyName)).IsRequired();
            b.Property(x => x.PharmacyPhone).HasColumnName(nameof(Provider.PharmacyPhone)).IsRequired();
            b.HasMany(x => x.WorkingTimes).WithOne().HasForeignKey(x => x.ProviderId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        });


        builder.Entity<Currency>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Currencies", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<City>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Cities", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Country>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Countries", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<VerficationCode>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "VerficationCodes", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        // For enable MultiTenancy in this table:
        builder.Entity<PatientProvider>().HasQueryFilter(e => e.Provider!.TenantId == CurrentTenant.Id);

        builder.Entity<Order>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Orders", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.OrderId).IsRequired().OnDelete(DeleteBehavior.ClientCascade);
            b.OwnsOne(x => x.DeliveryAddress);
        });

        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "OrderItems", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Order>().WithMany(x => x.Items).HasForeignKey(x => x.OrderId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Attachment>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Attachments", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Device>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Devices", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Notification>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Notifications", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<DawaaNeo.Services.Service>(b =>
        {
            b.ToTable(DawaaNeoConsts.DbTablePrefix + "Services", DawaaNeoConsts.DbSchema);
            b.ConfigureByConvention();
        });


    }
}
