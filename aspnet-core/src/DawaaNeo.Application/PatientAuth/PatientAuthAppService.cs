using DawaaNeo.Otps;
using DawaaNeo.ApiResponse;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using DawaaNeo.Patients;
using DawaaNeo.SMS;
using System.Globalization;
using DawaaNeo.Devices;


namespace DawaaNeo.PatientAuth
{
    public class PatientAuthAppService : ApplicationService , IPatientAuthAppService , ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly  IPatientRepository _patientRepository;
        private readonly  PatientManager _patientManager;
        private readonly IRepository<Otp,Guid> _otpRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IApiResponse  _apiResponse;
        private readonly IdentityUserManager  _userManager;
        private readonly ISmsAppService _smsSender;


        public PatientAuthAppService(IConfiguration configuration, IPatientRepository patientRepository, PatientManager patientManager, IDeviceRepository deviceRepository
            , IRepository<Otp, Guid> otpRepository, IApiResponse apiResponse, IdentityUserManager userManager, ISmsAppService smsSender
            )
        {
            _configuration = configuration;
            _patientRepository = patientRepository;
            _patientManager = patientManager;
            _otpRepository = otpRepository;
            _apiResponse = apiResponse;
            _userManager = userManager;
            _smsSender = smsSender;
            _deviceRepository = deviceRepository;
        }

        #region Login
        public async Task<Response<bool>> LogIn(PatientLoginDto input)
        {
            var verficationCode = _generateVerificationCode();
            bool patientExist = await _otpRepository.AnyAsync(p => p.MobileNumber == input.CountryCode + input.MobileNumber);
            if (!patientExist) {
                await _otpRepository.InsertAsync(new Otp(GuidGenerator.Create(),input.CountryCode+input.MobileNumber,verficationCode),true);
            }
            else
            {
                var oldOtp = await _otpRepository.FirstOrDefaultAsync(p =>
                p.MobileNumber == input.CountryCode + input.MobileNumber
                );
                if (oldOtp == null) throw new UserFriendlyException("Not FOund");
                oldOtp.Code = verficationCode;
                oldOtp.IsUsed = false;
                await _otpRepository.UpdateAsync(oldOtp);
            }
            //var msgIsSend = await _smsSender.SendSmsMessage(input.CountryCode+input.MobileNumber,verficationCode);
            //if (!msgIsSend) throw new UserFriendlyException("Error in send message");
            return _apiResponse.Success(true);
        }
        #endregion

        #region Auth
        public async Task<Response<LoginDto>> AuthenticateUser(PatientAuthDto input)
        {
            IdentityUser newUser;
            Guid userId = Guid.Empty;
            var otpCheck = await _checkCode(input);
            if (!otpCheck.Status) throw new UserFriendlyException("Not Found");
            var patient = await _patientRepository.FindAsync(p => p.MobileNumber == input.MobileNumber && p.CountryCode == input.CountryCode);
            var currentLanguageAsString = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var currentLanguage = currentLanguageAsString == "en" ? ApplicationLanguage.en : ApplicationLanguage.ar;
            var username = $"{input.CountryCode + input.MobileNumber}";
            var newEmail = $"{input.CountryCode + input.MobileNumber}@email";
             var password = "P@ssw0rd"; //_hashPassword(input.CountryCode + input.MobileNumber); For any one know password
            if (patient is null)
            {
                string customPatientId = _generateCustomPatientId();
                bool patientIdIsExist = await _patientRepository.AnyAsync(p => p.PatientId == customPatientId);
                while (patientIdIsExist)
                {
                    customPatientId = _generateCustomPatientId();
                    patientIdIsExist = await _patientRepository.AnyAsync(p => p.PatientId == customPatientId);
                }
                await _patientManager.CreateAsync(input.MobileNumber, input.CountryCode, customPatientId,input.Name,input.Dob);
                newUser = new IdentityUser(GuidGenerator.Create(),username,newEmail,null);
                newUser.SetIsActive(true);
                newUser.SetEmailConfirmed(true);
                await _userManager.CreateAsync(newUser,password);
                userId = newUser.Id;
                await CurrentUnitOfWork.CompleteAsync();
            }
            else
            {
               patient.CurrentLanguage = currentLanguage;
                await _patientRepository.UpdateAsync(patient,true);

            }
            var loginDto = await getToeknFromAuth(username,password);
            if (input.DeviceToken is not null)
            {
                await _deviceRepository.AddDevice(userId, input.DeviceToken);
            }
            return _apiResponse.Success(loginDto);
        }

        [HttpPost]
        public async Task<Response<LoginDto>> GetRefreshToken(RefreshTokenInput input)
        {
            var authority = _configuration["AuthServer:Authority"];
            var discoveryCache = new DiscoveryCache(authority);
            var disco = await discoveryCache.GetAsync();
            var httpClient = new Lazy<HttpClient>(() => new HttpClient());
            var response = await httpClient.Value.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "FirstProjet_App", // name from authorize in swagger
                RefreshToken = input.RefreshToken,
                Scope = "offline_access FirstProjet"
            });
            if (response.IsError && response.ErrorDescription.Contains("valid")) 
            {
                return _apiResponse.Unauthorized<LoginDto>(response.ErrorDescription);
            };

            LoginDto loginDto = new LoginDto
            {
                Token = response.AccessToken,
                RefreshToken = response.RefreshToken
            };
            return _apiResponse.Success(loginDto);
        }


        #endregion

        #region resend OTP
        public async Task<Response<bool>> ResendOtp(PatientLoginDto input)
        {
            var verificatioCode = _generateVerificationCode();
            var exist = await _otpRepository.FirstOrDefaultAsync(o => o.MobileNumber == input.CountryCode + input.MobileNumber);
            if (exist is null) throw new UserFriendlyException("Not Found");
            exist.Code = verificatioCode;
            await _otpRepository.UpdateAsync(exist);
            // Call send sms service:
            await _smsSender.SendSmsMessage(input.CountryCode + input.MobileNumber , verificatioCode);
            return _apiResponse.Success(true);
        }

        #endregion

        #region Methods
        public static string _hashPassword(string password)
        {
            string salt = "fdjfisdjfjsdlkf1f4ds65f4sd4424356145341d2as1d3as444f7dsffsd2asdwe";
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] saltedPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Array.Copy(saltBytes, saltedPasswordBytes, saltBytes.Length);
            Array.Copy(passwordBytes,0, saltedPasswordBytes, saltBytes.Length,passwordBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static string _generateCustomPatientId()
        {
            const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const int CodeLength = 6;
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomByte = new byte[CodeLength];
                rng.GetBytes(randomByte);
                char[] chars = new char[CodeLength];
                for (int i = 0; i < CodeLength; i++)
                {
                    int index = randomByte[i] % AllowedChars.Length;
                    chars[i] = AllowedChars[index];
                }
                return new string(chars);
            }
        }

        public static string _generateVerificationCode()
        {
            const string AllowedChars = "0123456789";
            const int CodeLength = 6;
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomByte = new byte[CodeLength];
                rng.GetBytes(randomByte);
                char[] chars = new char[CodeLength];
                for (int i = 0; i < CodeLength; i++)
                {
                    int index = randomByte[i] % AllowedChars.Length;
                    chars[i] = AllowedChars[index];
                }
                return new string(chars);
            }
        }

        public async Task<Response<bool>> _checkCode(PatientAuthDto dto)
        {
            var intOtpInterval = Int32.Parse(_configuration["otpInterval"]); // code valid for 2 Minute
            var otp = await _otpRepository.FindAsync(o => o.MobileNumber == dto.CountryCode + dto.MobileNumber && o.Code == dto.Code);
            if (otp is null) throw new UserFriendlyException("OTP Not Found");
            else if (otp.LastModificationTime is null)
            {
              if (otp.CreationTime.AddSeconds(intOtpInterval) < DateTime.Now)
              {
                throw new UserFriendlyException("Expired OTP");
              }
              else
              {
                otp.IsUsed = true;
                await _otpRepository.UpdateAsync(otp);
                return _apiResponse.Success(true);
              }
            }
            else if(otp.LastModificationTime != null && otp.LastModificationTime.Value.AddSeconds(intOtpInterval) < DateTime.Now)
                throw new UserFriendlyException("Expired OTP");
            else
            {
                otp.IsUsed = true;
                await _otpRepository.UpdateAsync(otp);
                return _apiResponse.Success(true);
            }

        }

        private async Task<LoginDto> getToeknFromAuth(string username,string password)
        {
            var authority = _configuration["AuthServer:Authority"];
            var discoveryCache = new DiscoveryCache(authority);
            var disco = await discoveryCache.GetAsync();
            var httpClient = new Lazy<HttpClient>(() => new HttpClient());
            var response = await httpClient.Value.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "DawaaNeo_App", // name from authorize in swagger
                UserName = username,
                Password = password,
                Scope = "offline_access DawaaNeo"
            });
            if (response.IsError) throw new UserFriendlyException(response.Error);
            LoginDto loginDto = new LoginDto
            {
                MobileNumber = username,
                Token = response.AccessToken,
                RefreshToken = response.RefreshToken
            };
            return loginDto;
        }

        #endregion
    }
}
