using DawaaNeo.Emailing;
using DawaaNeo.Permissions;
using DawaaNeo.Providers;
using DawaaNeo.QrCode;
using DawaaNeo.QrPdf;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using static DawaaNeo.Permissions.DawaaNeoPermissions;

namespace DawaaNeo.ProviderAuth
{
    public class ProviderAuthAppService : ApplicationService, IProviderAuthAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;

        private readonly IPermissionManager _permissionManager;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        private readonly ITenantManager _tenantManager;
        private readonly ITenantRepository _tenantRepository;
        private readonly IDistributedEventBus _distributedEventBus;

        private readonly ProviderManager _providerManager;
        private readonly IProviderRepository _providerRepository;
        private readonly IConfiguration _configuration;
        private readonly IBackgroundJobManager _backgroundJobManager;

        private readonly QrCodeService _qrCodeService;
        private readonly QrPdfService _qrPdfService;

        public ProviderAuthAppService(IdentityUserManager userManager, IdentityRoleManager roleManager,
            IPermissionManager permissionManager, IPermissionDefinitionManager permissionDefinitionManager,
            ITenantManager tenantManager, ITenantRepository tenantRepository,
            IDistributedEventBus distributedEventBus, ProviderManager providerManager, IProviderRepository providerRepository,
            IConfiguration configuration, IBackgroundJobManager backgroundJobManager,
            QrCodeService qrCodeService, QrPdfService qrPdfService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _permissionDefinitionManager = permissionDefinitionManager;
            _tenantManager = tenantManager;
            _tenantRepository = tenantRepository;
            _distributedEventBus = distributedEventBus;
            _providerManager = providerManager;
            _providerRepository = providerRepository;
            _configuration = configuration;
            _backgroundJobManager = backgroundJobManager;
            _qrCodeService = qrCodeService;
            _qrPdfService = qrPdfService;
        }

        public async Task<bool> Register(ProviderRegisterInfoDto input)
        {
            var isUnique = await _checkUniqueEmail(input.Email);
            if (isUnique == 0) throw new UserFriendlyException("EmailShouldBeUniqueMessage");

            var newTenantName = await _normalizeTenantName(input.PharmacyName);
            
            #region create new tenant
            var createdTenant = await _tenantManager.CreateAsync(newTenantName);
            await _tenantRepository.InsertAsync(createdTenant, true);

            await _distributedEventBus.PublishAsync(new TenantCreatedEto
            {
                Id = createdTenant.Id,
                Name = createdTenant.Name,
                Properties =
                {
                    {"AdminEmail","admin@abp.io" },
                    {"AdminPassword","1q2w3E*" }
                }
            });

            await _createTenantRoles(createdTenant.Id);
            #endregion

            #region create new user
            // Create new user in users table:
            var newUser = new IdentityUser(new Guid(), input.Email, input.Email, createdTenant.Id);
            newUser.SetIsActive(false);
            var result = await _userManager.CreateAsync(newUser, input.Password);

            // Assign role to use:
            using (CurrentTenant.Change(createdTenant.Id))
            {
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, DawaaNeoConsts.PharmacistRoleName);
                }
            }
            #endregion

            var workingTimes = ObjectMapper.Map<List<WorkingTimeDto>, List<WorkingTime>>(input.WorkingTimes);

            var provider = await _providerManager.CreateAsync(input.Email,input.PharmacyName,input.PharmacyPhone,
                input.LocationInfo.Latitude,input.LocationInfo.Longitude,
                input.LocationInfo.Address,input.LocationInfo.CityId,workingTimes,createdTenant.Id);

            if (CurrentUnitOfWork != null) await CurrentUnitOfWork.CompleteAsync();

            await SendVerificationEmail(input.Email,input.PharmacyName,createdTenant.Name);

            return true;
        }

        public virtual async Task SendVerificationEmail(string email,string pharmacyName,string tenantName)
        {

            using (DataFilter.Disable<IMultiTenant>())
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new UserFriendlyException("NotFound");
                }

                string confimationLink = await _generateEmailConformaitionLink(user);

                try
                {
                    await _backgroundJobManager.EnqueueAsync(
                    new EmailSendingArgs
                    {
                        Template = "Verification",
                        TargetEmail = email,
                        ConfirmationLink = confimationLink,
                        EmailPlaceHolder = user.Email,
                        TenantPlaceHolder = tenantName,
                        PharmacyNamePlaceHolder = pharmacyName
                    });
                }

                catch(Exception ex) { }
            }
        }

        public async Task<bool> Verify(VerifyCodeDto input)
        {
            using (DataFilter.Disable<IMultiTenant>())
            {
                IdentityUser user = await _userManager.FindByEmailAsync(input.Email);
                if (user is not null)
                {
                    input.Token = input.Token.Replace(" ", "+");
                    var result = await _userManager.ConfirmEmailAsync(user, input.Token);
                    if (result.Succeeded)
                    {
                        user.SetIsActive(true);
                        await _userManager.UpdateAsync(user);
                        var provider = ObjectMapper.Map<Provider, ProviderDto>(await _providerRepository.GetProviderByEmail(user.Email));
                        var pharmacyName = provider.PharmacyName;
                        var tenantName = await _tenantRepository.GetAsync(provider.TenantId);
                        await SendWelcomeEmail(user.Email, provider.Id, pharmacyName, tenantName.Name);
                        return true;
                    }
                }
                return false;
            }
        }

        public async Task SendWelcomeEmail(string email, Guid providerId ,string pharmacyName,string tenantName)
        {
            using (DataFilter.Disable<IMultiTenant>())
            {
                await _backgroundJobManager.EnqueueAsync(
                new EmailSendingArgs { 
                    Template = "Welcome", 
                    TargetEmail = email , 
                    EmailPlaceHolder = email , 
                    PharmacyNamePlaceHolder =  pharmacyName , 
                    TenantPlaceHolder = tenantName , 
                    ProviderId = providerId 
                }
                );
            }

        }

        private async Task _createTenantRoles(Guid tenantId)
        {
            var permissions = await _permissionDefinitionManager.GetPermissionsAsync();
            using (CurrentTenant.Change(tenantId))
            {
                string PharmacyRoleName = DawaaNeoConsts.PharmacistRoleName;
                var pharmacyRolePermission = permissions.Where(x => x.IsEnabled &&
                (x.Name.StartsWith(DawaaNeoPermissions.GroupName) || x.Name.StartsWith("AbpIdentity."))
                && x.MultiTenancySide != MultiTenancySides.Host);

                var createdRole = await _roleManager.CreateAsync(new IdentityRole(Guid.NewGuid(),
                    PharmacyRoleName, tenantId));

                foreach (var permission in pharmacyRolePermission)
                {
                    try { await _permissionManager.SetForRoleAsync(PharmacyRoleName, permission.Name, true);} catch { }
                }
            }
        }

        private async Task<int> _checkUniqueEmail(string email)
        {
            using (DataFilter.Disable<IMultiTenant>())
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null) { return 0; }
                else { return 1; }
            }
        }

        private async Task<string> _normalizeTenantName(string pharmacyName)
        {
            var newTenantName = pharmacyName;
            var tenant = await _tenantRepository.FindByNameAsync(newTenantName);
            if (tenant is not null)
            {
                var tenants = await _tenantRepository.GetListAsync();
                string pattern = @"" + pharmacyName + "[_]*\\d*";
                Regex rg = new Regex(pattern);
                var tenantWithTheSameStart = tenants.Where(tenant => rg.Matches(tenant.Name).Any());
                var lastTenant = tenantWithTheSameStart.Last();

                string patternNoTenantNameId = @"\A" + pharmacyName + "\\Z";
                Regex rgNoTenantNameId = new Regex(patternNoTenantNameId);
                string patternWithTenantNameId = @"" + pharmacyName + "[_]+\\d+";
                Regex rgWithTenantNameId = new Regex(patternNoTenantNameId);
                var tenantNameId = "";

                if (tenants.Where(tenant => rgWithTenantNameId.Matches(newTenantName).Any()).Count() > 0)
                {
                    tenantNameId = (rgNoTenantNameId.Matches(lastTenant.Name).Count()).ToString("00");
                }

                newTenantName = newTenantName + tenantNameId;

            }

            return newTenantName;
        }

        private async Task<string> _generateEmailConformaitionLink(IdentityUser user)
        {
            using (DataFilter.Disable<IMultiTenant>())
            {
                try
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string confirmLink = _configuration["App:AngularUrl"] +
                        _configuration["App:ConfirmationEmailLink"] + user.Email + "&c=" + token;
                    return confirmLink;
                }
                catch(Exception ex) {  }

                return "";
            }
        }

        public async Task ResendVerficationEmail(string targetEmail)
        {
            using (DataFilter.Disable<IMultiTenant>())
            {
                //find user by email
                var user = await _userManager.FindByEmailAsync(targetEmail.Replace(" ","+"));
                if (user == null) throw new UserFriendlyException("Not Found");
                string confirmationLink = await _generateEmailConformaitionLink(user);
                await _backgroundJobManager.EnqueueAsync(
                new EmailSendingArgs
                {
                    Template = "Verification",
                    TargetEmail = user.Email,
                    ConfirmationLink = confirmationLink
                }
                );
            }
        }

        // Downlaod QRCOde if already exist:
        public MemoryStream downloadQrCode()
        {
            var filePath = "..\\Acme.FirstProjet.Domain\\Emailing\\Templates\\qr_code" + ".pdf";
            var ms = new MemoryStream(System.IO.File.ReadAllBytes(filePath));
            return ms;
        }

        public async Task<IRemoteStreamContent> GenerateQrAndDownlaod()
        {
            var provider = await _providerRepository.FirstOrDefaultAsync(x => x.Email.ToLower() == CurrentUser.Email);
            var qrPath = await _qrCodeService.GenerateQrCode(provider!.Id);
            var pdfPath = await _qrPdfService.GenerateQrPdf(provider.Id);
            if (File.Exists(pdfPath))
            {
                var qrPdfStream = new RemoteStreamContent(new MemoryStream(System.IO.File.ReadAllBytes(pdfPath)), "qr_code" + provider.Id + ".pdf");
                File.Delete(pdfPath);
                if(File.Exists(qrPath)) { File.Delete(qrPath); }
                return qrPdfStream;

            }
            return null;
        }

        
    }
}
