using DawaaNeo.ApiResponse;
using DawaaNeo.Patients;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace DawaaNeo.Providers.Mobile
{
    public class ProviderMobileAppService : ApplicationService, IProviderMobileAppService
    {
        protected IProviderRepository _providerRepository;
        protected IPatientRepository _patientRepository;
        protected IPatientProviderRepository _patientProviderRepository;
        protected ProviderManager _providerManager;
        protected IApiResponse _apiResponse;
        protected IDataFilter _dataFilter;
        protected ICurrentPatient _currentPatient;

        public ProviderMobileAppService(
            IProviderRepository providerRepository,
            IPatientRepository patientRepository,
            IPatientProviderRepository patientProviderRepository,
            ProviderManager providerManager,
            IApiResponse apiResponse,
            IDataFilter dataFilter,
            ICurrentPatient currentPatient
            )
        {
            _apiResponse = apiResponse;
            _providerRepository = providerRepository;
            _patientRepository = patientRepository;
            _patientProviderRepository = patientProviderRepository;
            _providerManager = providerManager;
            _dataFilter = dataFilter;
            _currentPatient = currentPatient;
        }

        [Authorize]
        public async Task<Response<ProviderDto>> ScanQR(string providerCode)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                // Get Pharmacy Info:
                var pharmacy = ObjectMapper.Map<Provider,ProviderDto>(await _providerRepository.GetWithWorkTimesAsync(providerCode));
                if (pharmacy == null) throw new UserFriendlyException("Not Found");

                // add to my pharmacy:
                var currentUserFromToken = _currentPatient.getUserNameFromToken();
                var patient = await _patientRepository.GetPatientByMobileNumber(currentUserFromToken);
                var listOfUserPharmacy = await _patientProviderRepository.GetListForMobileAsync(currentUserFromToken);
                var items = ObjectMapper.Map<List<PatientProvider>, List<PharmacyInfoDto>>(listOfUserPharmacy);
                foreach (var item in items)
                {
                    if (item.Id == new Guid(providerCode))
                    {
                        var oldPharmacy = listOfUserPharmacy.Find(ph => ph.ProviderId == new Guid(providerCode));
                        oldPharmacy.AddingDate = DateTime.Now;
                        return _apiResponse.Success<ProviderDto>(pharmacy, "Scanned Pharmacy already added!!");
                    }
                }

                PatientProvider patientProvider = new PatientProvider(GuidGenerator.Create(),patient.Id,new Guid(providerCode),DateTime.Now,0);
                await _patientProviderRepository.InsertAsync(patientProvider);

                return _apiResponse.Success<ProviderDto>(pharmacy, "This Pharmacy has been added to your pharmacy list");
            }
        }
    }
}
