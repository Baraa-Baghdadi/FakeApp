using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DawaaNeo.SMS
{
    public class SmsAppService : ApplicationService, ISmsAppService
    {
        private readonly IConfiguration _configuration;
        public SmsAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendSmsMessage(string mobileNumber, string message)
        {
            try
            {
                string userName = _configuration["SMSUsername"];
                string password = _configuration["SMSPassword"];
                string senderName = _configuration["SMSSenderName"];

                string clientIdParameter = string.Empty;

                var checkStartCharacter = mobileNumber.Substring(0, 6);

                if (checkStartCharacter.Contains("+971"))
                {
                    mobileNumber = mobileNumber.Replace("+971", "971");
                }
                else if (checkStartCharacter.Contains("00971"))
                {
                    mobileNumber = mobileNumber.Remove(0, 5).Insert(0, "971");
                }

                string allMessage = $"Your Verification Code {message}";
                HttpClient client = new HttpClient();
                string link = $@"{_configuration["SMSUrl"]}?UserName={userName}&Password={password}&SenderName={senderName}&GSMMobileNumber={mobileNumber}&SMSText={allMessage}&clientid=2";
                var response = await client.PostAsync(link, null);
                if ((int) response.StatusCode == 200)
                {
                    return true;
                }
                else
                {
                    Logger.LogError(message: $@"{JsonConvert.SerializeObject(response)}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(message: $@"{JsonConvert.SerializeObject(ex)}");
                return false;
            }
        }
    }
}
