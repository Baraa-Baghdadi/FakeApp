using DawaaNeo.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace DawaaNeo.PatientAuth
{
    public interface IPatientAuthAppService : IApplicationService
    {
        Task<Response<bool>> LogIn(PatientLoginDto input);
        Task<Response<bool>> ResendOtp(PatientLoginDto input);
        Task<Response<LoginDto>> AuthenticateUser(PatientAuthDto input);
        Task<Response<LoginDto>> GetRefreshToken(RefreshTokenInput input);
    }
}
